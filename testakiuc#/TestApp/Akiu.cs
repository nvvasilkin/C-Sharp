using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AkiuLib
{
    class Akiu
    {
        public static string PtrToStringUtf8(IntPtr ptr) // ptr is nul-terminated
        {
            if (ptr == IntPtr.Zero)
                return "";
            int len = 0;
            while (System.Runtime.InteropServices.Marshal.ReadByte(ptr, len) != 0)
            {
                len++;
            }

            if (len == 0)
                return "";
            byte[] array = new byte[len];
            System.Runtime.InteropServices.Marshal.Copy(ptr, array, 0, len);
            return System.Text.Encoding.UTF8.GetString(array);
        }

            [DllImport("libakiu.dll")]
            static public extern IntPtr createAgentInstance();

            [DllImport("libakiu.dll")]
            static public extern void disposeAgent(IntPtr agentPointer);

            [DllImport("libakiu.dll",CharSet=CharSet.Auto)]
            static public extern bool authorizationAgent(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string sysNameAgent);

            [DllImport("libakiu.dll")]
            static public extern IntPtr getAgentSysName(IntPtr agentPointer);

            [DllImport("libakiu.dll",CharSet = CharSet.Auto)]
            static public extern bool registerAgent(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string fullNameAgent_ru, [MarshalAs(UnmanagedType.LPStr)] string fullNameAgent_fr, [MarshalAs(UnmanagedType.LPStr)] string sysNameAgent);

            [DllImport("libakiu.dll")]
            static public extern bool deleteAgent(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string fullNameAgent, [MarshalAs(UnmanagedType.LPStr)] string sysNameAgent);

            [DllImport("libakiu.dll")]
            static public extern bool registerParametr(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string oid, [MarshalAs(UnmanagedType.LPStr)] string name_ru, [MarshalAs(UnmanagedType.LPStr)] string name_fr, [MarshalAs(UnmanagedType.LPStr)] string measure_ru, [MarshalAs(UnmanagedType.LPStr)] string measure_fr, [MarshalAs(UnmanagedType.LPStr)] string min_warn_val, [MarshalAs(UnmanagedType.LPStr)] string max_warn_val, [MarshalAs(UnmanagedType.LPStr)] string min_crit_val, [MarshalAs(UnmanagedType.LPStr)] string max_crit_val);

            [DllImport("libakiu.dll")]
            static public extern bool setParameterValue(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string oid, [MarshalAs(UnmanagedType.LPStr)] string value);

            [DllImport("libakiu.dll")]
            static public extern bool getStatusRegisterAgent(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName);

            [DllImport("libakiu.dll")]
            static public extern bool getStatusRegisterParametr(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string oid);

            [DllImport("libakiu.dll")]
            static public extern bool getStatusSentParametr(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string oid);

            [DllImport("libakiu.dll")]
            static public extern bool setAgentStatus(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName, bool status);

            [DllImport("libakiu.dll")]
            static public extern bool setAgentPath(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName, [MarshalAs(UnmanagedType.LPStr)] string path);

            [DllImport("libakiu.dll")]
            static public extern bool setCritEvent(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName, [MarshalAs(UnmanagedType.LPStr)] string events);

            [DllImport("libakiu.dll")]
            static public extern bool checkCurrentConnectionAgentToDB(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName);

            [DllImport("libakiu.dll")]
            static public extern bool checkCurrentReconnectAgentToDB(IntPtr agentPointer, [MarshalAs(UnmanagedType.LPStr)] string agentSysName);

    }
}