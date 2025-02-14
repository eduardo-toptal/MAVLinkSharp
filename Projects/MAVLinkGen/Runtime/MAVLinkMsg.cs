
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics;
using System.Text;

#pragma warning disable CS8618
#pragma warning disable CS8603
#pragma warning disable CS1522

namespace MAVLinkBindings {

    #region MAVLinkMsgId
    /// <summary>
    /// Enumeration for message ids
    /// </summary>
    public enum MAVLinkMsgId {
        //%msg-id%
    }
    #endregion

    /// <summary>
    /// Class that describes a MAVLink Message Container
    /// </summary>    
    public class MAVLinkMsg {
        
        #region Pool
        /// <summary>
        /// Fetch a memory pooled message instance
        /// </summary>
        /// <returns></returns>
        static public MAVLinkMsg GetPool() {            
            MAVLinkMsg msg = null;
            lock(m_pool) if(m_pool.Count>0) { msg = m_pool[0]; m_pool.RemoveAt(0); }
            if(msg==null) msg = new MAVLinkMsg();
            return msg;
        }

        /// <summary>
        /// Returns a memory pooled message instance
        /// </summary>
        /// <returns></returns>
        static public void SetPool(MAVLinkMsg p_instance) {               
            lock(m_pool) if(!m_pool.Contains(p_instance)) { m_pool.Add(p_instance); }
        }

        /// <summary>
        /// Message Pool
        /// </summary>
        static public List<MAVLinkMsg> m_pool;
        #endregion

        /// <summary>
        /// CTOR.
        /// </summary>
        static MAVLinkMsg() {
            m_pool = new List<MAVLinkMsg>();
            for(int i=0;i<1000;i++) m_pool.Add(new MAVLinkMsg());
        }

        #region MessageID to PayloadLength
        /// <summary>
        /// Returns the message payload size generated from the definition XML
        /// </summary>
        static public byte GetMessagePayloadLength(int p_msg_id) {
            switch(p_msg_id) {
                //%msg-payload-length%
            }            
            return 0;
        }
        #endregion

        #region MessageID to Instance
        /// <summary>
        /// Returns the message instance to be populated w/ data
        /// </summary>
        static public IMAVLinkMessageData GetMessageInstance(int p_msg_id) {
            switch(p_msg_id) {
                //%msg-instance%
            }            
            return null;
        }
        #endregion

        #region class Signature
        /// <summary>
        /// Signature data structure
        /// </summary>
        public class Signature {

            /// <summary>
            /// Signature Link Id
            /// </summary>
            public byte linkId;

            /// <summary>
            /// Signature Timestamp 6 bytes
            /// </summary>
            public ulong timestamp;

            /// <summary>
            /// Signature Hash 6 bytes
            /// </summary>
            public ulong hash;

        }
        #endregion

        /// <summary>
        /// Flag that tells this header is valid.
        /// </summary>
        public bool valid { get { return version>0; } }

        /// <summary>
        /// MAVLink Version
        /// </summary>
        public int version;

        /// <summary>
        /// Payload Byte Length
        /// </summary>
        public byte payloadLength;

        /// <summary>
        /// Incompatibility Mask
        /// </summary>
        public byte incompatibilityFlags;

        /// <summary>
        /// Incompatibility Mask
        /// </summary>
        public byte compatibilityFlags;

        /// <summary>
        /// Sequence Index
        /// </summary>
        public byte sequence;

        /// <summary>
        /// System Id
        /// </summary>
        public byte systemId;

        /// <summary>
        /// System Id
        /// </summary>
        public byte componentId;

        /// <summary>
        /// Message Id
        /// </summary>
        public MAVLinkMsgId messageId; 

        /// <summary>
        /// Reference to the payload data
        /// </summary>
        public IMAVLinkMessageData data;

        /// <summary>
        /// Flag that tells there is signature info
        /// </summary>
        public bool isSigned { get { return (incompatibilityFlags & 0x1) != 0; } }

        /// <summary>
        /// Reference to signature data
        /// </summary>        
        public Signature signature { get; internal set; }

        /// <summary>
        /// Return the string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"MAVLinkMsg.{messageId} | {payloadLength}/{MAVLinkMsg.GetMessagePayloadLength((int)messageId)} bytes | sys: {systemId} comp: {componentId}";
        }

    }
}
