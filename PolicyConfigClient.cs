using System;
using System.Runtime.InteropServices;

[ComImport, Guid("568b9108-44bf-40b4-9006-86afee8b69ed")]
internal interface IPolicyConfigVista
{
    [PreserveSig]
    int SetDefaultEndpoint([In, MarshalAs(UnmanagedType.LPWStr)] string wszDeviceId, [In] int eRole);
}

[ComImport, Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
internal class CPolicyConfigVistaClient
{
    [return: MarshalAs(UnmanagedType.Interface)]
    [PreserveSig]
    public extern IPolicyConfigVista Activate();
}

public class PolicyConfigClient
{
    public void SetDefaultEndpoint(string deviceId, int role)
    {
        var config = new CPolicyConfigVistaClient();
        var policyConfig = config.Activate();
        Marshal.ThrowExceptionForHR(policyConfig.SetDefaultEndpoint(deviceId, role));
    }
}
