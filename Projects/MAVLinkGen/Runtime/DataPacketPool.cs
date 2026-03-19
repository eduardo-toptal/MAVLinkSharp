using System;
using System.Collections.Generic;
using System.IO;

namespace MAVLinkSharp.Runtime {

    /// <summary>
    /// Thread-safe pool and queue of Packet instances.
    ///
    /// The pool maintains a set of reusable buffers of fixed size.
    /// When Push(...) is called, incoming data is copied into a pooled
    /// packet buffer and the packet is queued.
    ///
    /// Pop(...) removes the oldest queued packet and transfers ownership
    /// of that packet to the caller. The caller must call Return(...)
    /// after processing the packet.
    ///
    /// Oversized packets allocate a temporary buffer which is not reused
    /// by the pool when returned.
    /// </summary>
    public sealed class DataPacketPool {

        /// <summary>
        /// Packet containing a reusable byte buffer and the valid byte length.
        /// </summary>
        public struct Packet {
            public byte[] buffer;
            public int length;
        }

        /// <summary>
        /// Synchronizes access to queue and pool.
        /// </summary>
        private readonly object m_lock = new();

        /// <summary>
        /// Free reusable packets.
        /// </summary>
        private readonly List<Packet> m_pool = new();

        /// <summary>
        /// Queued packets waiting to be popped.
        /// </summary>
        private readonly List<Packet> m_queue = new();

        /// <summary>
        /// Fixed buffer size used by pooled packets.
        /// </summary>
        private readonly int m_buffer_len;

        /// <summary>
        /// Initializes the packet pool.
        /// </summary>
        /// <param name="p_pool_count">Initial packet pool size.</param>
        /// <param name="p_buffer_len">Fixed buffer size for pooled packets.</param>
        public DataPacketPool(int p_pool_count = 128,int p_buffer_len = 1024) {

            if(p_pool_count <= 0) p_pool_count = 128;
            if(p_buffer_len <= 0) p_buffer_len = 1024;

            m_buffer_len = p_buffer_len;

            for(int i = 0; i < p_pool_count; i++) {
                Packet packet;
                packet.buffer = new byte[m_buffer_len];
                packet.length = 0;
                m_pool.Add(packet);
            }
        }

        /// <summary>
        /// Number of packets currently queued.
        /// </summary>
        public int Count { get { lock(m_lock) return m_queue.Count; } }

        /// <summary>
        /// Fixed buffer size used by the pool.
        /// </summary>
        public int BufferLength { get { return m_buffer_len; } }

        /// <summary>
        /// Copies a byte array into a pooled packet and queues it.
        /// </summary>
        public bool Push(byte[] p_data) {
            if(p_data == null || p_data.Length == 0) return false;
            return Push(p_data,p_data.Length);
        }

        /// <summary>
        /// Copies a region of a byte array into a pooled packet and queues it.
        /// </summary>
        public bool Push(byte[] p_data,int p_length) {

            if(p_data == null || p_length <= 0) return false;
            if(p_length > p_data.Length) p_length = p_data.Length;

            Packet packet = Rent(p_length);

            Buffer.BlockCopy(p_data,0,packet.buffer,0,p_length);
            packet.length = p_length;

            lock(m_lock) m_queue.Add(packet);

            return true;
        }

        /// <summary>
        /// Copies bytes from the current stream Position to Length
        /// into a pooled packet and queues it.
        /// </summary>
        public bool Push(Stream p_stream) {

            if(p_stream == null || !p_stream.CanRead) return false;

            long remaining = p_stream.Length - p_stream.Position;
            if(remaining <= 0) return false;
            if(remaining > int.MaxValue) return false;

            int length = (int)remaining;

            Packet packet = Rent(length);

            int offset = 0;

            while(offset < length) {
                int read = p_stream.Read(packet.buffer,offset,length - offset);
                if(read <= 0) break;
                offset += read;
            }

            if(offset <= 0) { Return(packet); return false; }

            packet.length = offset;

            lock(m_lock) m_queue.Add(packet);

            return true;
        }

        /// <summary>
        /// Pops the oldest queued packet.
        /// Caller owns the packet until Return(...) is called.
        /// </summary>
        public bool Pop(out Packet p_packet) {
            p_packet = default;
            lock(m_lock) {
                if(m_queue.Count <= 0) return false;
                p_packet = m_queue[0];
                m_queue.RemoveAt(0);
            }
            return true;
        }

        /// <summary>
        /// Returns a packet to the free pool if it matches the fixed pool size.
        /// Oversized packets are discarded.
        /// </summary>
        public void Return(Packet p_packet) {
            p_packet.length = 0;
            if(p_packet.buffer == null) return;
            if(p_packet.buffer.Length != m_buffer_len) return;
            lock(m_lock) m_pool.Add(p_packet);
        }

        /// <summary>
        /// Clears all queued packets and returns pooled-size packets to the pool.
        /// </summary>
        public void Clear() {
            lock(m_lock) {
                for(int i = 0; i < m_queue.Count; i++) {
                    Packet packet = m_queue[i];
                    packet.length = 0;
                    byte[] b = packet.buffer;
                    if(b == null) continue;
                    if(b.Length == m_buffer_len) m_pool.Add(packet);
                }
                m_queue.Clear();
            }
        }

        /// <summary>
        /// Rents a packet from the free pool or allocates a new one.
        /// </summary>
        private Packet Rent(int p_required_length) {
            Packet packet;
            lock(m_lock) {
                if(m_pool.Count > 0) {
                    packet = m_pool[0];
                    m_pool.RemoveAt(0);
                }
                else {
                    packet.buffer = new byte[m_buffer_len];
                    packet.length = 0;
                }
            }
            if(packet.buffer == null || p_required_length > packet.buffer.Length) packet.buffer = new byte[p_required_length];

            packet.length = 0;

            return packet;
        }
    }
}
