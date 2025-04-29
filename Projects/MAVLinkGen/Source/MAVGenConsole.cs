using MAVLinkSharp.Runtime;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using static MAVLinkGen.MAVGenConsole.Definition;
using static MAVLinkGen.MAVGenConsole.Definition.Enumeration;

namespace MAVLinkGen {
    internal class MAVGenConsole {

        static string CSEnumTemplate = 
@"        
namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// %description%
    /// </summary>    
    public enum %name% {
        %entry%
    }

}
";

        static string CSstructTemplate = 
@"        
using System.IO;
using System.Runtime.InteropServices;
using MAVLinkSharp.Runtime;

#pragma warning disable CS0675

namespace MAVLinkSharp.Bindings {

    /// <summary>
    /// %description%
    /// </summary>    
    public struct %name% : IMAVLinkMessageData {

        /// <summary>
        /// Message Id Associated w/ this Struct
        /// </summary>    
        public int GetId() { return %message-id%; }

        %field%    

        #region CTOR
        /// <summary>
        /// Instantiates a new %name%
        /// </summary>    
        /*
        public %name%() {
            Init();
        }
        */
        public void Init() {
            //%ctor-field%
        }
        #endregion

        #region Read Buffer
        /// <summary>
        /// Reads the data from Buffer into this struct
        /// </summary>    
        public int Read(byte[] p_buffer,int p_offset=0) {
            int    l = %payload-length%;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals
            int[]  LS8  = MAVLinkCRC.U8_LSH8, LS16 = MAVLinkCRC.U8_LSH16, LS24 = MAVLinkCRC.U8_LSH24, LS32 = MAVLinkCRC.U8_LSH32, LS40 = MAVLinkCRC.U8_LSH40, LS48 = MAVLinkCRC.U8_LSH48, LS56 = MAVLinkCRC.U8_LSH56;
            Span<byte> b = p_buffer.AsSpan(p_offset);            
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            %buffer-read%            
            return p;
        }
        #endregion

        #region Write Buffer
        /// <summary>
        /// Writes the message data into a Buffer
        /// </summary>    
        public int Write(byte[] p_buffer,int p_offset=0) {
            int    l = %payload-length%;
            //Assert Range
            if((p_buffer.Length - p_offset) < l) return 0; 
            //Locals            
            Span<byte> b = p_buffer.AsSpan(p_offset);
            int        p = 0;            
            //byte[] b = p_buffer;
            //int    p = p_offset;
            %buffer-write%
            return p;
        }
        #endregion

        #region Read Stream
        /// <summary>
        /// Reads the struct data from a stream
        /// </summary>
        /// <param name=""p_stream""></param>
        /// <returns></returns>
        public int Read(Stream p_stream) {
            Stream ss = p_stream;
            if(ss==null) return 0;
            int l = %payload-length%;
            if(ss.Length - ss.Position < l) return 0;
            byte[] b;            
            long p = ss.Position;
            if(ss is MemoryStream) {
                MemoryStream ms = ( MemoryStream ) ss;
                b = ms.GetBuffer();
            }
            else {
                b = new byte[l];
                p = 0;
                ss.Read(b,0,l);                
            }
            return Read(b,(int)p);
        }
        #endregion

        #region Write Stream
        /// <summary>
        /// Writes the struct data into a Stream
        /// </summary>
        /// <param name=""p_stream""></param>
        /// <returns></returns>
        public int Write(Stream p_stream) {
            Stream ss = p_stream;
            if(ss==null) return 0;
            MemoryStream ms = new MemoryStream();
            int c = Read(ms);
            ms.Position=0;
            ms.CopyTo(ss);            
            ms.Close();
            return c;
        }
        #endregion

    }

}
";

        #region enum CmdVarType
        /// <summary>
        /// Types of arguments
        /// </summary>
        public enum CmdVarType {
            None = 0,
            Path,
            File,
            OutputPath,
            TemplatesPath
        }
        #endregion

        #region class Arguments
        /// <summary>
        /// Startup Arguments
        /// </summary>
        public class Arguments {

            /// <summary>
            /// Message Def Path
            /// </summary>
            public string path;

            /// <summary>
            /// Directory of the path
            /// </summary>
            public DirectoryInfo pathDir;

            /// <summary>
            /// File Name within Path
            /// </summary>
            public string file;

            /// <summary>
            /// REsult output path
            /// </summary>
            public string outputPath;

            /// <summary>
            /// Result output directory.
            /// </summary>
            public DirectoryInfo outputDir;

            /// <summary>
            /// Path to the template files.
            /// </summary>
            public string templatesPath;

            /// <summary>
            /// Templates directory.
            /// </summary>
            public DirectoryInfo templatesDir;

            /// <summary>
            /// CTOR.
            /// </summary>
            public Arguments() {
                path          = "";
                pathDir       = null;
                file          = "";
                outputPath    = "";
                outputDir     = null;
                templatesPath = "";
                templatesDir  = null;
            }
            
        }
        #endregion

        #region class Definition
        /// <summary>
        /// Describes a MAVLink Message Definition File.
        /// </summary>
        public class Definition {

            #region static
            /// <summary>
            /// Formats a string into camelcase
            /// </summary>
            /// <param name="p_name"></param>
            /// <param name="p_cap_firt"></param>
            /// <param name="p_suffix"></param>
            /// <returns></returns>
            static public string FormatCamelCase(string p_name,bool p_cap_firt,string p_suffix="") {
                string n     = p_name;
                string n_sfx = p_suffix;
                if(string.IsNullOrEmpty(n)) return "";
                string[] tkl = n.Split('_');                
                for(int i=0;i<tkl.Length;i++) {
                    string vs = tkl[i];
                    vs = vs.ToLower();
                    bool will_cap = true;
                    if(i<=0) if(!p_cap_firt) will_cap=false;
                    if(will_cap) vs = char.ToUpper(vs[0]) + vs.Substring(1);
                    tkl[i] = vs;
                }
                return $"{string.Join("",tkl)}{n_sfx}";
            }

            /// <summary>
            /// Formats an enumeration name to camel case 
            /// </summary>
            /// <param name="p_name"></param>
            /// <param name="p_cap_first"></param>
            /// <param name="p_suffix"></param>
            /// <returns></returns>
            static public string FormatEnumCamelCase(string p_name,bool p_cap_first,string p_suffix="") {
                bool   enum_sfx  = (p_name.ToLower().EndsWith("flag") || p_name.ToLower().EndsWith("flags"));
                string res = FormatCamelCase(p_name,true,enum_sfx ? "" : "Flags");
                res = res.Replace("Mav"    ,"MAV"    );
                res = res.Replace("MAVlink","MAVLink");
                return res;
            }
            #endregion

            #region class Enumeration
            /// <summary>
            /// Describes an enumeration data type.
            /// </summary>
            public class Enumeration {

                #region class Entry
                /// <summary>
                /// Describes a single enumeration data entry.
                /// </summary>
                public class Entry {

                    #region class Param
                    /// <summary>
                    /// Parameter Data for MAV_CMD enum entries.
                    /// </summary>
                    public class Param {

                        /// <summary>
                        /// Index in the Array
                        /// </summary>
                        public int index;

                        /// <summary>
                        /// Param Label
                        /// </summary>
                        public string label;

                        /// <summary>
                        /// Param Description
                        /// </summary>
                        public string description;

                        /// <summary>
                        /// CTOR
                        /// </summary>
                        public  Param() {
                            index=0;
                            label="";
                            description="";
                        }

                        /// <summary>
                        /// Parses this node xml
                        /// </summary>
                        /// <param name="p_node"></param>
                        public void ParseNode(XmlNode p_node) {
                            XmlNode n = p_node;                        
                            if(n==null) return;
                            string vs;
                            description = n.InnerText.Replace("\r\n", "\n");
                            foreach(XmlAttribute it in n.Attributes) {
                                switch(it.Name) {
                                    case "index": vs    = it.Value.Trim(); int.TryParse(vs,out index); break;
                                    case "label": label = it.Value;     break;
                                }
                            }                            
                        }

                        /// <summary>
                        /// String Value
                        /// </summary>
                        /// <returns></returns>
                        public override string ToString() {
                            return $"Param [{index}] {label}";
                        }

                    }
                    #endregion

                    /// <summary>
                    /// Item Name
                    /// </summary>
                    public string name;
                    /// <summary>
                    /// Item Value
                    /// </summary>
                    public int value;

                    /// <summary>
                    /// Item Description
                    /// </summary>
                    public string description;

                    /// <summary>
                    /// List of parameters relevant to the given message
                    /// </summary>
                    public List<Param> parameters;

                    /// <summary>
                    /// CTOR.
                    /// </summary>
                    public Entry() {
                        name        = "";
                        value       = 0;
                        description = "";
                        parameters = new List<Param>();
                    }

                    /// <summary>
                    /// Returns the formated name.
                    /// </summary>
                    /// <param name="p_cap_first"></param>
                    /// <param name="p_suffix"></param>
                    /// <returns></returns>
                    public string GetCamelCaseName(bool p_cap_first=false,string p_suffix="") {
                        return FormatCamelCase(name,p_cap_first,p_suffix);
                    }

                    /// <summary>
                    /// Parses the XML for this Entry
                    /// </summary>
                    /// <param name="p_node"></param>
                    public void ParseNode(XmlNode p_node) {
                        XmlNode n = p_node;                        
                        if(n==null) return;
                        string vs;
                        foreach(XmlAttribute it in n.Attributes) {
                            switch(it.Name) {
                                case "name":   name = it.Value;     break;
                                case "value":  vs   = it.InnerText.Trim(); int.TryParse(vs,out value); break;
                            }
                        }
                        foreach(XmlNode it in n.ChildNodes) {
                            switch(it.Name) {                                
                                case "description": description = it.InnerText.Replace("\r\n","\n"); break;
                                case "param":     { Param p = new Param(); p.ParseNode(it); parameters.Add(p); } break;
                            }
                        }
                        string param_s = "";
                        List<string> param_tkl = new List<string>();
                        for(int i=0;i<parameters.Count; i++) {
                            string lb = parameters[i].label.Replace(" ","");
                            if(string.IsNullOrEmpty(lb)) continue;
                            param_tkl.Add($"{i}: {lb.PadRight(18)}");                            
                        }
                        param_s = string.Join(" | ",param_tkl);
                        if(!string.IsNullOrEmpty(param_s))description = $"[Params {param_s}] {description}";
                    }
                    /// <summary>
                    /// String Value
                    /// </summary>
                    /// <returns></returns>
                    public override string ToString() {
                        return $"Entry {name} = {value}";
                    }
                }
                #endregion

                /// <summary>
                /// Enumeration Name
                /// </summary>
                public string name;

                /// <summary>
                /// Enumeration Description.
                /// </summary>
                public string description;

                /// <summary>
                /// List of available entries.
                /// </summary>
                public List<Entry> entries;

                /// <summary>
                /// CTOR.
                /// </summary>
                public Enumeration() {
                    name        = "";
                    description = "";
                    entries     = new List<Entry>();
                }

                /// <summary>
                /// Returns the formated name.
                /// </summary>
                /// <param name="p_cap_first"></param>
                /// <param name="p_suffix"></param>
                /// <returns></returns>
                public string GetCamelCaseName(bool p_cap_first=false,string p_suffix="") {
                    return FormatCamelCase(name,p_cap_first,p_suffix);
                }

                /// <summary>
                /// Returns the max size of all entries names.
                /// </summary>
                /// <returns></returns>
                public int GetEntryMaxNameLength() {
                    int len=0;
                    foreach(Entry e in entries) len = Math.Max(len,e.name.Length);
                    return len;
                }

                /// <summary>
                /// Parses an 'enum' node from definition file.
                /// </summary>
                /// <param name="p_node"></param>
                public void ParseNode(XmlNode p_node) {
                    XmlNode n = p_node;                    
                    if(n==null) return;
                    foreach(XmlAttribute it in n.Attributes) {
                        switch(it.Name) {
                            case "name": name = it.Value; break;                            
                        }
                    }                    
                    foreach(XmlNode it in n.ChildNodes) {
                        switch(it.Name) {
                            case "description": description = it.InnerText.Replace("\r\n","\n"); break;
                            case "entry":     { Entry e = new Entry(); e.ParseNode(it); entries.Add(e); } break;
                        }
                    }
                    SortEntries();
                }

                /// <summary>
                /// Sorts entries by value.
                /// </summary>
                public void SortEntries() {
                    entries.Sort(delegate(Entry a,Entry b){ return a.value < b.value ? -1 : 1; });
                }

                /// <summary>
                /// String Value
                /// </summary>
                /// <returns></returns>
                public override string ToString() {
                    return $"Enum {name}";
                }


            }
            #endregion

            #region class Message
            /// <summary>
            /// Describes a MAVLink Message
            /// </summary>
            public class Message {

                #region class Field
                /// <summary>
                /// Describes a field of a MAVLinkMessage
                /// </summary>
                public class Field {

                    /// <summary>
                    /// Index of the field in the Message Field List
                    /// </summary>
                    public int index;

                    /// <summary>
                    /// Field Name
                    /// </summary>
                    public string name;

                    /// <summary>
                    /// Data Type
                    /// </summary>
                    public string type;

                    /// <summary>
                    /// C# Type
                    /// </summary>
                    public string csType;

                    /// <summary>
                    /// Function to read this field from a stream
                    /// </summary>
                    public string streamReadOp;

                    /// <summary>
                    /// Function to read this field from a buffer
                    /// </summary>
                    public string bufferReadOp;

                    /// <summary>
                    /// Size of the type
                    /// </summary>
                    public int typeLength;

                    /// <summary>
                    /// Size of the type times array length
                    /// </summary>
                    public int totalLength;

                    /// <summary>
                    /// Flag that tells it is an array
                    /// </summary>
                    public bool isArray;

                    /// <summary>
                    /// Flag that tells the numeric field is an enum
                    /// </summary>
                    public bool isEnum;

                    /// <summary>
                    /// Flag that tells the field is an extension
                    /// </summary>
                    public bool isExtension;

                    /// <summary>
                    /// The type of enum of the field
                    /// </summary>
                    public string enumType;

                    /// <summary>
                    /// Length in case of array type.
                    /// </summary>
                    public int arrayLength;

                    /// <summary>
                    /// Value when not valid.
                    /// </summary>
                    public string defaultValue;

                    /// <summary>
                    /// Field Description.
                    /// </summary>
                    public string description;

                    /// <summary>
                    /// CTOR.
                    /// </summary>
                    public Field() {
                        name         = "";
                        type         = "";
                        enumType     = "";
                        defaultValue = "";
                        description  = "";
                        isArray      = false;
                        isEnum       = false;
                        isExtension  = false;
                        totalLength  = 0;
                        typeLength   = 0;
                        index        = -1;
                    }

                    /// <summary>
                    /// Returns the formated name.
                    /// </summary>
                    /// <param name="p_cap_first"></param>
                    /// <param name="p_suffix"></param>
                    /// <returns></returns>
                    public string GetCamelCaseName(bool p_cap_first=false,string p_suffix="") {
                        return FormatCamelCase(name,p_cap_first,p_suffix);
                    }

                    /// <summary>
                    /// Parses the 'field' node.
                    /// </summary>
                    /// <param name="p_node"></param>
                    public void ParseNode(XmlNode p_node) {
                        XmlNode n = p_node;                    
                        if(n==null) return;
                        description = n.InnerText.Replace("\r\n","\n");                        
                        foreach(XmlAttribute it in n.Attributes) {
                            switch(it.Name) {                                
                                case "name":     name         = it.Value; break;
                                case "type":     type         = it.Value; break;
                                case "invalid":  defaultValue = it.Value; break;
                                case "enum":     enumType     = it.Value; isEnum = true; break;
                            }
                        }                
                        if(!string.IsNullOrEmpty(type)) {
                            Match rgx = Regex.Match(type,@"(.+)(\[[0-9]+\])");
                            string t_name = type;
                            if(rgx.Success) {
                                t_name = rgx.Groups[1].Value;
                                string t_len = rgx.Groups[2].Value.Replace("[","").Replace("]","").Trim();
                                isArray = true;
                                int.TryParse(t_len, out arrayLength);
                            }
                            int len_arr = arrayLength<=0 ? 1 : arrayLength;
                            t_name = t_name.ToLower().Trim();                            
                            csType = t_name;    
                            type   = t_name;
                            switch(t_name) {

                                case "char":                    csType = "char";    typeLength = 1; totalLength = typeLength * len_arr; streamReadOp = "ReadChar  ()"; bufferReadOp = "(b[p++])";break;
                                case "uint8_t":                 csType = "byte";    typeLength = 1; totalLength = typeLength * len_arr; streamReadOp = "ReadByte  ()"; bufferReadOp = "(b[p++])";break;                                
                                case "int8_t":                  csType = "sbyte";   typeLength = 1; totalLength = typeLength * len_arr; streamReadOp = "ReadSByte ()"; bufferReadOp = "(b[p++])";break;

                                case "uint16_t":                csType = "ushort";  typeLength = 2; totalLength = typeLength * len_arr; streamReadOp = "ReadUInt16()"; bufferReadOp = "(b[p++] | LS8[b[p++]])"; break;
                                case "int16_t":                 csType = "short";   typeLength = 2; totalLength = typeLength * len_arr; streamReadOp = "ReadInt16 ()"; bufferReadOp = "(b[p++] | LS8[b[p++]])"; break;

                                case "uint32_t":                csType = "uint";    typeLength = 4; totalLength = typeLength * len_arr; streamReadOp = "ReadUInt32()"; bufferReadOp = "(b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]])"; break;
                                case "int32_t":                 csType = "int";     typeLength = 4; totalLength = typeLength * len_arr; streamReadOp = "ReadInt32 ()"; bufferReadOp = "(b[p++] | LS8[b[p++]] | LS16[b[p++]] | LS24[b[p++]])"; break;

                                case "uint64_t":                csType = "ulong";   typeLength = 8; totalLength = typeLength * len_arr; streamReadOp = "ReadUInt64()"; bufferReadOp = "((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]])"; break;
                                case "int64_t":                 csType = "long";    typeLength = 8; totalLength = typeLength * len_arr; streamReadOp = "ReadInt64 ()"; bufferReadOp = "((ulong)b[p++] | (ulong)LS8[b[p++]] | (ulong)LS16[b[p++]] | (ulong)LS24[b[p++]] | (ulong)LS32[b[p++]] | (ulong)LS40[b[p++]] | (ulong)LS48[b[p++]] | (ulong)LS56[b[p++]])"; break;

                                case "float":                   csType = "float";   typeLength = 4; totalLength = typeLength * len_arr; streamReadOp = "ReadSingle()"; bufferReadOp = "MemoryMarshal.Read<float >(b.Slice(p,4)); p+=4"; break;
                                case "double":                  csType = "double";  typeLength = 8; totalLength = typeLength * len_arr; streamReadOp = "ReadDouble()"; bufferReadOp = "MemoryMarshal.Read<double>(b.Slice(p,8)); p+=8"; break;
                                //case "float":                   csType = "float";   typeLength = 4; totalLength = typeLength * len_arr; streamReadOp = "ReadSingle()"; bufferReadOp = "BitConverter.ToSingle(b,p); p+=4;"; break;
                                //case "double":                  csType = "double";  typeLength = 8; totalLength = typeLength * len_arr; streamReadOp = "ReadDouble()"; bufferReadOp = "BitConverter.ToDouble(b,p); p+=8;"; break;
                                case "uint8_t_mavlink_version": csType = "byte";    typeLength = 1; totalLength = typeLength * len_arr; streamReadOp = "ReadByte  ()"; bufferReadOp = "(b[p++])"; type = "uint8_t"; break;
                            }                            
                        }       
                        
                        
                    }

                    /// <summary>
                    /// String Value
                    /// </summary>
                    /// <returns></returns>
                    public override string ToString() {
                        return $"Field {name}:{type}{(string.IsNullOrEmpty(enumType) ? "" : $":{enumType}")}";
                    }

                }
                #endregion

                /// <summary>
                /// Message Id
                /// </summary>
                public int id;

                /// <summary>
                /// Name
                /// </summary>
                public string name;

                /// <summary>
                /// Message Description
                /// </summary>
                public string description;

                /// <summary>
                /// List of fields.
                /// </summary>
                public List<Field>  fields;

                /// <summary>
                /// Returns the total payload size.
                /// </summary>
                public byte payloadLength { get { byte v=0; for(int i=0;i<fields.Count;i++) v+=(byte)fields[i].totalLength; return v; } }

                /// <summary>
                /// CRC of this Message
                /// </summary>
                public byte crc;

                /// <summary>
                /// CTOR.
                /// </summary>
                public Message() {
                    id          = -1;
                    name        = "";
                    description = "";
                    fields      = new List<Field>();
                    crc         = 0;
                }

                /// <summary>
                /// Returns the formated name.
                /// </summary>
                /// <param name="p_cap_first"></param>
                /// <param name="p_suffix"></param>
                /// <returns></returns>
                public string GetCamelCaseName(bool p_cap_first=false,string p_suffix="") {
                    return FormatCamelCase(name,p_cap_first,p_suffix);
                }

                /// <summary>
                /// Returns the max size of all entries names.
                /// </summary>
                /// <returns></returns>
                public int GetFieldMaxNameLength() {
                    int len=0;
                    foreach(Field f in fields) len = Math.Max(len,f.name.Length);
                    return len;
                }

                /// <summary>
                /// Parses the 'message' node
                /// </summary>
                /// <param name="p_node"></param>
                public void ParseNode(XmlNode p_node) {
                    XmlNode n = p_node;                    
                    if(n==null) return;
                    string vs;
                    foreach(XmlAttribute it in n.Attributes) {
                        switch(it.Name) {
                            case "id":   vs   = it.Value; int.TryParse(vs,out id); break;
                            case "name": name = it.Value; break;
                        }
                    }
                    bool is_extension = false;
                    int idx=0;
                    foreach(XmlNode it in n.ChildNodes) {
                        switch(it.Name) {
                            case "description":   description = it.InnerText.Replace("\r\n","\n");         break;
                            case "field":       { Field f = new Field(); f.index = idx++; f.isExtension = is_extension; f.ParseNode(it); fields.Add(f); } break;
                            case "extensions":    is_extension=true; break;
                        }
                    }
                    
                    //Sorting Logic
                    //https://mavlink.io/en/guide/serialization.html
                    fields.Sort(delegate(Field a,Field b) {
                        int  la = a.typeLength , lb = b.typeLength; 
                        int  ia = a.index       , ib = b.index; 
                        bool xa = a.isExtension , xb = b.isExtension;                        
                        //Extension goes Last
                        if( xa && !xb) return  1;
                        if(!xa &&  xb) return -1;
                        //When equal sort by XML order
                        if(xa && xb) return ia<ib ? -1 : 1;
                        //Bigger Types First 8b -> 4b -> 2b -> 1b
                        if(la>lb) return -1;                        
                        if(la<lb) return  1;
                        //When equal sort by xml order
                        return ia<ib ? -1 : 1;                        
                    });
                    //Re-index
                    for(int i=0;i<fields.Count;i++) fields[i].index = i;
                    //Assign CRC
                    crc = GetCRC();                     
                    //DEBUG CRC
                    //Compare w/ mavlinkpy debug logs on crc generation to see field orders and also 'message_info' from mavgen
                    //Console.WriteLine($"{id.ToString("00000")}, {(name+",").PadRight(32)} , {crc}");
                }

                /// <summary>
                /// Generates the CRC Extra for the Message
                /// </summary>
                /// <returns></returns>
                internal byte GetCRC() {         
                    
                    ushort crc16;
                    List<object> field_data = new List<object>();
                    field_data.Add(name + " ");
                    for(int i=0;i<fields.Count;i++) {
                        Field it_f = fields[i];    
                        if(it_f.isExtension) continue;
                        field_data.Add(it_f.type + " ");
                        field_data.Add(it_f.name + " ");
                        if(!it_f.isArray) continue;
                        field_data.Add((byte)it_f.arrayLength);
                    }
                    crc16 = MAVLinkCRC.GetCRC(field_data);
                    return MAVLinkCRC.GetCRCExtra(crc16);                    
                }

                /// <summary>
                /// String Value
                /// </summary>
                /// <returns></returns>
                public override string ToString() {
                    return $"Message {id.ToString()}:{name}";
                }

            }
            #endregion

            /// <summary>
            /// Reference to the definition file.
            /// </summary>
            public FileInfo file;

            /// <summary>
            /// Path to the parsing.
            /// </summary>
            public DirectoryInfo directory;

            /// <summary>
            /// Definition Version
            /// </summary>
            public int version;

            /// <summary>
            /// Dialect Version
            /// </summary>
            public int dialect;

            /// <summary>
            /// File Includes
            /// </summary>
            public List<FileInfo> includes;

            /// <summary>
            /// List of enumerations.
            /// </summary>
            public List<Enumeration> enums;

            /// <summary>
            /// List of messages definitions.
            /// </summary>
            public List<Message> messages;

            /// <summary>
            /// CTOR.
            /// </summary>
            public Definition(FileInfo p_file,string p_path) {
                file      = p_file;
                directory = new DirectoryInfo(p_path);
                enums     = new List<Enumeration>();
                messages  = new List<Message>();
                includes  = new List<FileInfo>();
                version   = 0;
                dialect   = 0;
            }

            /// <summary>
            /// Parses the definition file as XML
            /// </summary>
            public void ParseXML() {
                Console.WriteLine($"MAVGen> Parsing {file.FullName}");
                XmlDocument def_doc = new XmlDocument();
                def_doc.Load(file.FullName);                
                ParseNode(def_doc.DocumentElement);
            }

            /// <summary>
            /// Parses the nodes
            /// </summary>
            /// <param name="p_node"></param>
            public void ParseNode(XmlNode p_node) {
                XmlNode n = p_node;
                if(n==null) return;
                string vs;
                switch(n.Name) {
                    case "version": vs = n.InnerText.Trim(); int.TryParse(vs,out version); break;
                    case "dialect": vs = n.InnerText.Trim(); int.TryParse(vs,out dialect); break;

                    case "include": {
                        FileInfo include_file = new FileInfo(directory.FullName + "/"+n.InnerText);
                        if(!include_file.Exists) { Console.WriteLine($"MAVGen> Error Parsing [{currentFile.FullName}] include file [{include_file.FullName}] Not Found!"); break; }
                        includes.Add(include_file);
                    }
                    break;

                    case "enum": {
                        Enumeration en = new Enumeration();
                        en.ParseNode(n);
                        enums.Add(en);
                    }
                    break;

                    case "message": {
                        Message msg = new Message();
                        msg.ParseNode(n); 
                        messages.Add(msg);
                    }
                    break;
                }
                foreach(XmlNode it in n.ChildNodes) { ParseNode(it); }
            }

            /// <summary>
            /// String Value
            /// </summary>
            /// <returns></returns>
            public override string ToString() {
                return $"Definition [{file.Name}]";
            }

        }
        #endregion

        /// <summary>
        /// Reference to the app args.
        /// </summary>
        static public Arguments args;

        /// <summary>
        /// Reference to the current file.
        /// </summary>
        static public FileInfo currentFile;

        /// <summary>
        /// List of parsed definitions.
        /// </summary>
        static public List<Definition> definitions;

        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="p_args"></param>
        /// <returns></returns>
        static public int Main(string[] p_args) {

            //Locals
            Match rgxm;
            //Create arguments data
            args = new Arguments();
            //Arguments as tokens
            List<string> tkl = new List<string>(p_args);
            ///Current CmdVar Flaag
            CmdVarType var_flag = CmdVarType.None;
            //Extract tokens and process vars
            while(tkl.Count > 0) {
                //Args Token
                string s_var = tkl[0];
                //Assert
                s_var = s_var.Trim();
                tkl.RemoveAt(0);
                //Check if variable key
                switch(s_var) {
                    case "-p": var_flag = CmdVarType.Path;          continue;
                    case "-f": var_flag = CmdVarType.File;          continue;
                    case "-o": var_flag = CmdVarType.OutputPath;    continue;
                    case "-t": var_flag = CmdVarType.TemplatesPath; continue;
                }
                //Process next value
                switch(var_flag) {
                    case CmdVarType.Path:          { args.path          = s_var; } break;
                    case CmdVarType.File:          { args.file          = s_var; } break;
                    case CmdVarType.OutputPath:    { args.outputPath    = s_var; } break;
                    case CmdVarType.TemplatesPath: { args.templatesPath = s_var; } break;
                }
                //Reset for next variable parsing
                var_flag = CmdVarType.None;
            }
            //Assert Directories and File
            DirectoryInfo root_dir = new DirectoryInfo(Environment.CurrentDirectory);            
            if(string.IsNullOrEmpty(args.path)) args.path = $"{root_dir.FullName}/Definitions/v1.0";
            args.pathDir = new DirectoryInfo(args.path);
            if(!args.pathDir.Exists) { throw new DirectoryNotFoundException($"MAVLink Definitions Path [{args.pathDir.FullName}] Not Found!"); }
            FileInfo target_file = new FileInfo(args.pathDir.FullName+"/"+args.file);
            if(!target_file.Exists) { throw new FileNotFoundException($"MAVLink Message File [{target_file.FullName}] Not Found!"); }
            //File Parsing Stack
            List<FileInfo> file_stack = new List<FileInfo>();
            //Add root file
            file_stack.Add(target_file);    
            //Create definition list
            definitions = new List<Definition>();
            //Iterate File Stack
            while(file_stack.Count > 0) {
                FileInfo it = file_stack[0];
                file_stack.RemoveAt(0);
                currentFile = it;                                
                Definition def_data = new Definition(it,args.pathDir.FullName);
                def_data.ParseXML();                
                definitions.Add(def_data);
                file_stack.AddRange(def_data.includes);
            }
            //Join all messages and enums
            List<Definition.Enumeration> list_enums    = new List<Definition.Enumeration>();
            List<Definition.Message>     list_messages = new List<Definition.Message>();
            foreach(Definition it in definitions) {
                list_enums.AddRange(it.enums);
                list_messages.AddRange(it.messages);
            }            
            //Runtime Files
            DirectoryInfo runtime_dir = new DirectoryInfo(args.outputPath+"/Runtime/");
            if(runtime_dir.Exists) runtime_dir.Delete(true);
            runtime_dir.Create();
            //Assert Output Location
            if(string.IsNullOrEmpty(args.templatesPath)) throw new DirectoryNotFoundException("Templates Folder Not Found!");
            DirectoryInfo templates_dir = new DirectoryInfo(args.templatesPath);
            if(!templates_dir.Exists) throw new DirectoryNotFoundException("Templates Folder Not Found!");
            args.templatesDir = templates_dir;
            //Copy Template Files
            FileInfo[] template_srcs = templates_dir.GetFiles("*.cs");
            for(int i=0;i<template_srcs.Length;i++) { 
                FileInfo it = template_srcs[i];
                string file_contents = File.ReadAllText(it.FullName);
                switch(it.Name) {

                    #region MAVLinkCRC
                    case "MAVLinkCRC.cs": {

                        //Write CRC Extra fields for each message type, sampled from the XML file data.

                        rgxm = Regex.Match(file_contents,@"^.*//%message-crc%", RegexOptions.Multiline);
                        if(!rgxm.Success) break;
                        List<Tuple<int,byte,string>> msg_crcs = new List<Tuple<int, byte,string>>();
                        for(int j=0;j<definitions.Count;j++) {
                            Definition it_def = definitions[j];
                            for(int k=0;k<it_def.messages.Count;k++) {
                                Definition.Message it_m = it_def.messages[k];
                                msg_crcs.Add(new (it_m.id,it_m.crc,it_m.name));
                            }
                        }  
                        msg_crcs.Sort(delegate(Tuple<int,byte,string> a,Tuple<int,byte,string> b){ 
                            return a.Item1<b.Item1 ? -1 : 1;
                        });
                        
                        for(int j=0;j<msg_crcs.Count;j++) {
                            int    msg_id  = msg_crcs[j].Item1;
                            byte   msg_crc = msg_crcs[j].Item2;
                            string msg_n   = msg_crcs[j].Item3; 
                            string next_tk  = j >= msg_crcs.Count-1 ? "" : $"\n{rgxm.Value}";
                            string tk       = rgxm.Value.Replace($"//%message-crc%",$"case {msg_id.ToString().PadRight(6)}: return {(msg_crc+";").PadRight(6)} //{msg_n}{next_tk}");
                            file_contents = file_contents.Replace(rgxm.Value,tk);                    
                        }
                    }
                    break;
                    #endregion

                    #region MAVLinkMsg
                    case "MAVLinkMsg.cs": {


                        //Write the enumeration for MessageIds name = value

                        rgxm = Regex.Match(file_contents,@"^.*//%msg-id%", RegexOptions.Multiline);

                        if(!rgxm.Success) break;
                        List<Tuple<int,string,string>> msg_ids = new List<Tuple<int, string,string>>();
                        for(int j=0;j<list_messages.Count;j++) {
                            Definition.Message it_m = list_messages[j];
                            msg_ids.Add(new (it_m.id,it_m.GetCamelCaseName(true),it_m.description.Replace("\n"," | ")));
                        }  
                        msg_ids.Sort(delegate(Tuple<int,string,string> a,Tuple<int,string,string> b){ 
                            return a.Item1<b.Item1 ? -1 : 1;
                        });

                        for(int j=0;j<msg_ids.Count;j++) {
                            int    enum_id  = msg_ids[j].Item1;
                            string enum_n   = msg_ids[j].Item2;
                            string enum_d   = msg_ids[j].Item3;
                            string next_tk  = j >= msg_ids.Count-1 ? "" : $"\n{rgxm.Value}";
                            string tk       = rgxm.Value.Replace($"//%msg-id%",$"{enum_n.PadRight(42)} = {enum_id.ToString().PadRight(6)}, //{enum_d}{next_tk}");
                            file_contents = file_contents.Replace(rgxm.Value,tk);
                        }

                        //Write the instantiation of 'new msg-struct()' per message id

                        rgxm = Regex.Match(file_contents,@"^.*//%msg-instance%", RegexOptions.Multiline);

                        if(!rgxm.Success) break;
                        List<Tuple<int,string,string>> msg_instances = new List<Tuple<int, string,string>>();
                        for(int j=0;j<list_messages.Count;j++) {
                            Definition.Message it_m = list_messages[j];
                            msg_instances.Add(new (it_m.id,it_m.GetCamelCaseName(true,"Data"),it_m.description.Replace("\n"," | ")));
                        }  
                        msg_instances.Sort(delegate(Tuple<int,string,string> a,Tuple<int,string,string> b){ 
                            return a.Item1<b.Item1 ? -1 : 1;
                        });

                        for(int j=0;j<msg_instances.Count;j++) {
                            int    msg_id   = msg_instances[j].Item1;
                            string msg_n    = msg_instances[j].Item2;                            
                            string next_tk  = j >= msg_instances.Count-1 ? "" : $"\n{rgxm.Value}";
                            string tk       = rgxm.Value.Replace($"//%msg-instance%",$"case {msg_id.ToString().PadRight(6)}: return new {(msg_n+"();").PadRight(6)}{next_tk}");
                            file_contents = file_contents.Replace(rgxm.Value,tk);
                        }

                        //Writes payload length per message-id

                        rgxm = Regex.Match(file_contents,@"^.*//%msg-payload-length%", RegexOptions.Multiline);
                        if(!rgxm.Success) break;
                        List<Tuple<int,byte,string>> msg_payload_lengths = new List<Tuple<int, byte,string>>();
                        for(int j=0;j<definitions.Count;j++) {
                            Definition it_def = definitions[j];
                            for(int k=0;k<it_def.messages.Count;k++) {
                                Definition.Message it_m = it_def.messages[k];
                                msg_payload_lengths.Add(new (it_m.id,it_m.payloadLength,it_m.name));
                            }
                        }  
                        msg_payload_lengths.Sort(delegate(Tuple<int,byte,string> a,Tuple<int,byte,string> b){ 
                            return a.Item1<b.Item1 ? -1 : 1;
                        });
                        
                        for(int j=0;j<msg_payload_lengths.Count;j++) {
                            int    msg_id  = msg_payload_lengths[j].Item1;
                            byte   msg_pll = msg_payload_lengths[j].Item2;
                            string msg_n   = msg_payload_lengths[j].Item3;
                            string next_tk  = j >= msg_payload_lengths.Count-1 ? "" : $"\n{rgxm.Value}";
                            string tk       = rgxm.Value.Replace($"//%msg-payload-length%",$"case {msg_id.ToString().PadRight(6)}: return {(msg_pll+";").PadRight(6)}  //{msg_n}{next_tk}");
                            file_contents = file_contents.Replace(rgxm.Value,tk);                    
                        }
                    }
                    break;
                    #endregion

                }

                Console.WriteLine($"MAVGen> Copying Runtime Template {it.FullName}");
                File.WriteAllText($"{runtime_dir.FullName}{it.Name}",file_contents);
                //it.CopyTo($"{runtime_dir.FullName}{it.Name}");
            }

            //Sort Enums and Definitions
            list_enums.Sort(DefinitionEnumSort);
            list_messages.Sort(DefinitionMessageSort);

            //Assert Output Location
            if(string.IsNullOrEmpty(args.outputPath)) args.outputPath = Environment.CurrentDirectory+"/Output";
            DirectoryInfo output_dir = new DirectoryInfo(args.outputPath);
            if(!output_dir.Exists) { output_dir.Create(); }
            args.outputDir = output_dir;
            //Create path for enums and primitive types.
            DirectoryInfo types_dir = new DirectoryInfo(args.outputPath+"/Types/");
            if(types_dir.Exists) types_dir.Delete(true);
            types_dir.Create();
            DirectoryInfo messages_dir = new DirectoryInfo(args.outputPath+"/Messsages/");
            if(messages_dir.Exists) messages_dir.Delete(true);
            messages_dir.Create();

            List<string> desc_lines;
            string tpl;
            MatchCollection tpl_vars;
            
            //Write Enumeration Files
            foreach(Definition.Enumeration it in  list_enums) {
                tpl = CSEnumTemplate; 
                tpl_vars = Regex.Matches(tpl,"%[a-zA-Z]+%");

                string enum_name = Definition.FormatEnumCamelCase(it.name,true,"");
                string enum_rpl  = it.GetCamelCaseName(true,"");
                
                foreach(Match it_var in tpl_vars) {
                    switch(it_var.Value) {

                        #region %description%
                        case "%description%": {
                            rgxm = Regex.Match(tpl,@"^.*/// %description%", RegexOptions.Multiline);
                            if(!rgxm.Success) break;
                            desc_lines = new List<string>(it.description.Split('\n'));
                            for(int i=0;i<desc_lines.Count;i++) {
                                string desc_l        = desc_lines[i];                                
                                string desc_next_tk  = i >= desc_lines.Count-1 ? "" : $"\n{rgxm.Value}";
                                string desc_tk       = rgxm.Value.Replace($"%description%",$"{desc_l}{desc_next_tk}");
                                tpl = tpl.Replace(rgxm.Value,desc_tk);                    
                            }  
                        }
                        break;
                        #endregion

                        #region %name%
                        case "%name%": {
                            tpl = tpl.Replace($"%name%",enum_name);
                        }
                        break;
                        #endregion

                        #region %entry%
                        case "%entry%": {
                            int    entry_pad = it.GetEntryMaxNameLength();
                            rgxm = Regex.Match(tpl,@"^.*%entry%", RegexOptions.Multiline);
                            for(int i=0;i<it.entries.Count;i++) {
                                Definition.Enumeration.Entry it_e = it.entries[i];
                                string entry_next_tk = i >= it.entries.Count-1 ? "" : $"\n{rgxm.Value}";                                
                                desc_lines = new List<string>(it_e.description.Split('\n'));
                                for(int k=0;k<desc_lines.Count;k++) {
                                    string vs = desc_lines[k];
                                    vs = vs.Trim();
                                    if (string.IsNullOrEmpty(vs)) desc_lines.RemoveAt(k--); else desc_lines[k] = vs;
                                }
                                string entry_desc = $"  //{string.Join(" | ",desc_lines)}";                                
                                string entry_name = it_e.GetCamelCaseName(true,"");
                                string name_rpl   = it.name;
                                switch(it.name) {
                                    case "MAV_COMPONENT": name_rpl = "MAV_COMP_ID"; break;                        
                                }
                                entry_name = entry_name.Replace($"{enum_rpl}","");
                                entry_name = entry_name.Replace($"{enum_rpl}s","");
                                if(Regex.IsMatch(entry_name,@"^[0-9]+.*", RegexOptions.Multiline)) entry_name = $"_{entry_name}";
                                string entry_value = it_e.value.ToString();
                                entry_value += i>=it.entries.Count-1 ? "" : ",";
                                tpl = tpl.Replace($"%entry%",$"{entry_name.PadRight(entry_pad)} = {entry_value.PadRight(11)}{entry_desc}{entry_next_tk}");
                            }
                        }
                        break;
                        #endregion

                    }
                }                
                string file_path = $"{types_dir.FullName}/{it.name}.cs";
                File.WriteAllText(file_path,tpl);
            }

            //Write Struct Message Files
            foreach(Definition.Message it in  list_messages) {
                tpl = CSstructTemplate; 
                tpl_vars = Regex.Matches(tpl,@"%[a-zA-Z\-]+%");

                string msg_name = it.GetCamelCaseName(true,"Data");
                string msg_rpl  = it.GetCamelCaseName(true,"");
                
                foreach(Match it_var in tpl_vars) {
                    switch(it_var.Value) {

                        #region %description%
                        case "%description%": {
                            rgxm = Regex.Match(tpl,@"^.*/// %description%", RegexOptions.Multiline);
                            if(!rgxm.Success) break;
                            desc_lines = new List<string>(it.description.Split('\n'));

                            for(int i=0;i<desc_lines.Count;i++) {
                                string desc_l        = desc_lines[i].Trim();                                
                                string desc_next_tk  = i >= desc_lines.Count-1 ? "" : $"\n{rgxm.Value}";
                                string desc_tk       = rgxm.Value.Replace($"%description%",$"{desc_l}{desc_next_tk}");
                                tpl = tpl.Replace(rgxm.Value,desc_tk);                    
                            }  
                        }
                        break;
                        #endregion

                        #region %name%
                        case "%name%": {
                            tpl = tpl.Replace($"%name%",msg_name);
                        }
                        break;
                        #endregion

                        #region "%message-id%"
                        case "%message-id%": {
                            tpl = tpl.Replace($"%message-id%",it.id.ToString());
                        }
                        break;
                        #endregion

                        #region %field%
                        case "%field%": {
                            int    field_pad = it.GetFieldMaxNameLength()+1;
                            rgxm = Regex.Match(tpl,@"^.*%field%", RegexOptions.Multiline);

                            int type_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;                                
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.isArray) field_type += "[]";
                                type_pad = Math.Max(type_pad,field_type.Length);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                
                                desc_lines = new List<string>(it_f.description.Split('\n'));
                                for(int k=0;k<desc_lines.Count;k++) {
                                    string vs = desc_lines[k].Trim();
                                    vs = vs.Trim();
                                    if (string.IsNullOrEmpty(vs)) desc_lines.RemoveAt(k--); else desc_lines[k] = vs;
                                }
                                string field_desc = $"  //{string.Join(" | ",desc_lines)}";                                
                                string field_name = $"{it_f.GetCamelCaseName(true)};";
                                string field_type = it_f.csType;                                
                                if(it_f.isEnum)  field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.isArray) field_type += "[]";
                                if(it_f.name == "fixed") field_name = $"@{field_name}";

                                tpl = tpl.Replace($"%field%",$"public {field_type.PadRight(type_pad+1)} {field_name.PadRight(field_pad)}  {field_desc}{field_next_tk}");
                            }
                        }
                        break;
                        #endregion

                        #region %payload-length%
                        case "%payload-length%": {
                            tpl = tpl.Replace($"%payload-length%",it.payloadLength.ToString());
                        }
                        break;
                        #endregion

                        #region %ctor-field%
                        case "%ctor-field%": {
                            
                            rgxm = Regex.Match(tpl,@"^.*%ctor-field%", RegexOptions.Multiline);

                            int type_pad = 0;
                            int name_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;
                                string field_name = it_f.name;                                
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                type_pad = Math.Max(type_pad,field_type.Length);
                                name_pad = Math.Max(name_pad,field_name.Length+5);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string op_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                                                                                
                                string field_name = $"{it_f.GetCamelCaseName(true)}";
                                string field_type = it_f.csType;              
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.name == "fixed") field_name = $"@{field_name}";
                                
                                string op_var     = field_name;
                                string op_arr_len = it_f.arrayLength.ToString().PadLeft(3);

                                string op_line = $"default({field_type.PadRight(type_pad)})";
                                if(it_f.isArray) {
                                    op_line = $"new {field_type}[{op_arr_len}]";
                                }
                                tpl = tpl.Replace($"//%ctor-field%",$"{op_var.PadRight(name_pad)} = {op_line};{op_next_tk}");
                            }
                        }
                        break;
                        #endregion

                        #region %buffer-read%
                        case "%buffer-read%": {
                            
                            rgxm = Regex.Match(tpl,@"^.*%buffer-read%", RegexOptions.Multiline);

                            int type_pad = 0;
                            int name_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;
                                string field_name = it_f.name;                                
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                type_pad = Math.Max(type_pad,field_type.Length);
                                name_pad = Math.Max(name_pad,field_name.Length+5);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string op_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                                                                                
                                string field_name = $"{it_f.GetCamelCaseName(true)}";
                                string field_type = it_f.csType;              
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.name == "fixed") field_name = $"@{field_name}";

                                string op_var     = field_name + (it_f.isArray ? "[i]" : "");                                                                
                                string op_cmd     = it_f.bufferReadOp;
                                string op_arr_len = it_f.arrayLength.ToString().PadRight(3);                                
                                string op_line = $"{op_var.PadRight(name_pad)} = ({field_type.PadRight(type_pad)}) {op_cmd};";
                                if(it_f.isArray) {
                                    op_line = $"for(int i=0;i<{op_arr_len};i++) {{ {op_line} }}";
                                }
                                tpl = tpl.Replace($"%buffer-read%",$"{op_line}{op_next_tk}");
                            }
                        }
                        break;
                        #endregion

                        #region %buffer-write%
                        case "%buffer-write%": {
                            
                            rgxm = Regex.Match(tpl,@"^.*%buffer-write%", RegexOptions.Multiline);

                            int type_pad = 0;
                            int name_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;
                                string field_name = it_f.name;                                
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                type_pad = Math.Max(type_pad,field_type.Length);
                                name_pad = Math.Max(name_pad,field_name.Length+5);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string op_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                                                                                                                
                                string field_name = $"{it_f.GetCamelCaseName(true)}";
                                string field_type = it_f.csType;              
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.name == "fixed") field_name = $"@{field_name}";
                                
                                string op_var     = field_name + (it_f.isArray ? "[i]" : "");                                
                                string op_arr_len = it_f.arrayLength.ToString().PadLeft(3);

                                int rsh_c = 0;
                                int rsh_v = 0;

                                switch(it_f.csType) {
                                    case "char"   : rsh_c= 1; break;
                                    case "byte"   : rsh_c= 1; break;
                                    case "sbyte"  : rsh_c= 1; break;
                                    case "ushort" : rsh_c= 2; break;
                                    case "short"  : rsh_c= 2; break;
                                    case "uint"   : rsh_c= 4; break;
                                    case "int"    : rsh_c= 4; break;                                    
                                    case "ulong"  : rsh_c= 8; break;
                                    case "long"   : rsh_c= 8; break;
                                    case "float"  : rsh_c= 1; break;
                                    case "double" : rsh_c= 1; break;
                                }

                                string op_line;
                                
                                int pad_arr = 0;

                                if(it_f.isArray) {
                                    op_line = $"for(int i=0;i<{op_arr_len};i++) {{";
                                    pad_arr = 4;
                                    tpl = tpl.Replace($"%buffer-write%",$"{op_line}{$"\n{rgxm.Value}"}");
                                }                                
                                
                                

                                for(int j=0;j<rsh_c;j++) {                                        
                                    switch(it_f.csType) {
                                        case "float" : op_line = $"{"".PadRight(pad_arr)}MemoryMarshal.Write(b.Slice(p, 4), ref {op_var.PadRight(name_pad)}); p+=4;"; break;
                                        case "double": op_line = $"{"".PadRight(pad_arr)}MemoryMarshal.Write(b.Slice(p, 8), ref {op_var.PadRight(name_pad)}); p+=8;"; break;
                                        default: {
                                            //Low -> High Bytes
                                            string rsh_op   = $">>{rsh_v.ToString().PadRight(2)}";
                                            int    rsh_pad  = rsh_c<=1 ? 0 : 6;
                                            string op_cast  = rsh_c>=8 ? "long" : "int";
                                            string op_inner = rsh_v<=0 ? $"{"".PadRight(rsh_pad)}{op_var}" : $"({op_cast}){op_var}{rsh_op}";
                                            op_line = $"{"".PadRight(pad_arr)}b[p++] = (byte)({op_inner});";
                                            rsh_v+=8;
                                        }
                                        break;
                                    }

                                    string op_bin_next_tk = it_f.isArray ? $"\n{rgxm.Value}" : (j<(rsh_c-1) ? $"\n{rgxm.Value}" : op_next_tk);

                                    tpl = tpl.Replace($"%buffer-write%",$"{op_line}{op_bin_next_tk}");
                                }                                    

                                if(it_f.isArray) {
                                    op_line = $"}}";
                                    tpl = tpl.Replace($"%buffer-write%",$"{op_line}{$"{op_next_tk}"}");
                                }
                                
                            }
                        }
                        break;
                        #endregion

                        #region %stream-write%
                        case "%stream-write%": {
                            
                            rgxm = Regex.Match(tpl,@"^.*%stream-write%", RegexOptions.Multiline);

                            int type_pad = 0;
                            int name_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;
                                string field_name = it_f.name;
                                //if(it_f.isArray) field_type += "[]";
                                //if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                type_pad = Math.Max(type_pad,field_type.Length);
                                name_pad = Math.Max(name_pad,field_name.Length);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string ss_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                                                                                
                                string field_name = $"{it_f.GetCamelCaseName(true)}";
                                string field_type = it_f.csType;                                
                                if(it_f.name == "fixed") field_name = $"@{field_name}";
                                
                                string ss_op_var = field_name;
                                if(it_f.isArray) ss_op_var += $"[i]";
                                ss_op_var = ss_op_var.PadRight(name_pad);
                                string ss_op = $"bw.Write(({field_type.PadRight(type_pad)}){ss_op_var});";                                
                                if(it_f.isArray) {
                                    ss_op = $"for(int i=0;i<{it_f.arrayLength.ToString().PadRight(3)};i++) {ss_op}";
                                }
                                tpl = tpl.Replace($"%stream-write%",$"{ss_op}{ss_next_tk}");
                            }
                        }
                        break;
                        #endregion

                        #region %stream-read%
                        case "%stream-read%": {
                            
                            rgxm = Regex.Match(tpl,@"^.*%stream-read%", RegexOptions.Multiline);

                            int type_pad = 0;
                            int name_pad = 0;

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string field_type = it_f.csType;
                                string field_name = it_f.name;                                
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                type_pad = Math.Max(type_pad,field_type.Length);
                                name_pad = Math.Max(name_pad,field_name.Length);
                            }

                            for(int i=0;i<it.fields.Count;i++) {
                                Definition.Message.Field it_f = it.fields[i];
                                string ss_next_tk = i >= it.fields.Count-1 ? "" : $"\n{rgxm.Value}";                                                                                                
                                string field_name = $"{it_f.GetCamelCaseName(true)}";
                                string field_type = it_f.csType;              
                                if(it_f.isEnum) field_type = Definition.FormatEnumCamelCase(it_f.enumType,true,"");
                                if(it_f.name == "fixed") field_name = $"@{field_name}";
                                
                                string ss_op_var = field_name;
                                if(it_f.isArray) ss_op_var += $"[i]";
                                ss_op_var = ss_op_var.PadRight(name_pad);
                                string br_method = it_f.streamReadOp;
                                
                                string ss_op = $"{ss_op_var} = ({field_type.PadRight(type_pad)}) br.{br_method};";
                                if(it_f.isArray) {
                                    ss_op = $"for(int i=0;i<{it_f.arrayLength.ToString().PadRight(3)};i++) {ss_op}";
                                }
                                tpl = tpl.Replace($"%stream-read%",$"{ss_op}{ss_next_tk}");
                            }
                        }
                        break;
                        #endregion

                    }
                }                
                
                string file_path = $"{messages_dir.FullName}/{it.name}.cs";
                File.WriteAllText(file_path,tpl);
            }

            //Runtime
            {
                //string file_path = $"{runtime_dir.FullName}/IMAVLinkMessageData.cs";
                //File.WriteAllText(file_path,CSStructInterfaceTemplate);
            }

            //Success
            return 0;

        }

        static private int DefinitionEnumSort   (Definition.Enumeration a,Definition.Enumeration b) { return string.Compare(a.name,b.name); }
        static private int DefinitionMessageSort(Definition.Message a,Definition.Message b) { return a.id < b.id ? -1 : 1; }

    }
}
