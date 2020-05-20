# Windows��������Ĳ鿴�ͷ��� �����о�����ƿ�������

## ��������

```
OS : Microsoft Windows 7 (6.1.7601 Service Pack 1 Build 7601)
IDE: Microsoft Visual Studio Community 2019 (16.5.5)
```

## �������

������Ҫ�������û��Ѻõ�ͼ�ν��棬��������`C/C++/C#`�����ֿ�ѡ�ı��������ѡ����`C#`��ʹ��`C#`������`VB`һ��ʹ�÷ḻ�Ŀؼ�ʵ��ԭ��`Windows`���Ƶ�`UI`�����ӻ����֣�ʹ��`.NET Framework`���ṩ���ڶ�`API`����Ч�ر�д���룬����Ϥ`C/C++`�ĳ���Ա��˵����`C#`��дʵ�õĳ���û�����Ե����ѡ�

�������ڳ���ǰ�˿�����`C#`������Ϥ��������ǰ�ڴ�����ʱ�仨��ѧϰ`C# Winform`��ʵ�ÿؼ���ǰ�˽����ʵ�֡�������`Autoruns`������Ϊģ�壬�����ڿؼ���ѡ���ϲ�û��ѡ�������ѡ���Ҫ������ʾ��������Ŀؼ���`ListView`�ؼ�������`Autoruns`������ʾ5����Ϣ���ֱ��ǣ���������Ŀ���ơ���������Ŀ���������򷢲��ߡ�����·��������޸�ʱ�䡣

���л����˴�����ʱ�����������ͼ��չʾ��ע���ļ���·��������ǰ�еķ�Χ��ʾ�������е���������⡣

![q1](../images/q1.png)

Ĭ�ϵ�`ListView`�ǲ�֧��������ʾЧ���ģ��ı�����ʱ���Զ��ضϣ������������ʾע���·����˵�ǲ����Ѻõģ���Ҫչʾ������ע���·����ҪΪ��һ�з���ܳ������򣬶���ʾЧ����˵�Ƚ���⣬��������Ϊ������ʾЧ��Ӧ��Ҫ�ﵽ�����ܱ���Э��

��ѯ����ʦ����ʹ����`ListView`�ؼ��Զ����ػ�Ĺ���ʵ����������չʾЧ�����ü���ռ��4������ʾ�����ճ�������չʾЧ������ͼ��ʾ��

![after-ui](../images/5-16.png)

������ǰ�˽�����ƵĹ���������Ϊ�Ƚ����ѵĲ�����ͼ���չʾ������Ŀ�������Ϳؼ��Զ����ػ档

������ǰ�˽����չʾ��ʹ�õ���`API`��
- ʹ����`System.Drawing.Icon`���`ExtractAssociatedIcon`�������ļ�����ȡͼ��
- ʹ����`System.IO.FileInfo`����ȡ�ļ������ļ�·�������ļ��޸�ʱ�����Ϣ
- ʹ����`System.Security.Cryptography.X509Certificates.X509Certificate`����ȡ�ļ���֤�飬��֤����ȡ��������Ϣ
- ʹ����`IWshRuntimeLibrary`�еĹ�������ȡ��ݷ�ʽ��ָ����ļ�

��Щ�й����ļ�����ͷ������ֲ��طǳ���ɢ�����Ҹе���Щ������Ϊ����ΪӦ����һ������ר�Ÿ�����Щ�ļ�Ԫ���ݵ���Ϣ�Ķ�ȡ�����������������ͼ�ν����Ҽ������ܹ������й��ļ���������Ϣһ�������ݡ�

ʹ��`IWshRuntimeLibrary`�еĹ�������ȡ��ݷ�ʽ��ָ����ļ���֪��һ�������⣬���ҵĵ������н�����������������`C:\Program Files (x86)`�µ��ļ��������`C:\Program Files`�µ��ļ��������Ҷ����ʹ���������������ǡ�

## ����������

### Logon������Ŀ¼������ע�������

SysinternalsSuite��Autostart�����г��˽�Ϊ��ϸ��������Ŀ¼�Ͱ�����������ע��������ȽϺ��ҷ����г���������Ŀ¼ֻ������Ŀ¼������ʦPPT��������ֱ�Ϊ��
- `%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`
- `%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup`

��ͼ��`%USERPROFILE%`��������autorunsû�гɹ���������֪����Ϊʲô��

�����ҵ�windows�ں˰汾��6��`C:\Documents and Settings`��һ��`C:\Users`�����ӡ�

![q2](../images/q2.png)

�������������������ʾ�˺ܶ�PPT��û���ᵽ�ġ��Ұ�Autoruns����Ĭ�����ص�ѡ����ʾ��������`Logon`��һ���ڵ����е�ע�����������ڸ�¼�С�

��ʵ��ע���ֵ�����ݽ�����ʱ����ע�⵽��Щֵ������ֻ�м򵥵�һ���ļ�������Ҫ��Windows����ϵͳ�Ļ�������`PATH`����ȥѰ�ҡ���ʵ������߼���ʱ���ҷ�����һ���쳣��������飬������`"C:\\Windows\\System32"`���Ŀ¼��ԭ���`Autoruns`�����ҵ��Ŀ�ִ���ļ�����`C#`��ʹ��API��������ȴ�Ҳ�������һ��ʼ��Ϊ��Ȩ�޲����������ù���Ա���к����Ҳ���������������������`visual studio`Ϊ�ҵ���Ŀ�Զ���ѡ��`prefer 32 bit`ѡ�������������Ŀ�ִ���ļ���32λ�ļܹ����У�����`"C:\\Windows\\System32"`�ᱻ[�ض���](https://docs.microsoft.com/zh-cn/windows/win32/winprog64/file-system-redirector?redirectedfrom=MSDN)��`"C:\\Windows\\SysWOW64"`�������Ҳ����ļ�����`prefer 32 bit`ѡ��ȡ�����������ˡ�

���ڲ������������ע�����Ŀʵ����̫�࣬�Ҿ������һ�����ؿ����ѡ�


### Services��ϵͳ����

ͨ��Servicesϵͳ����ʵ������������Ŀȫ����ע����`HKLM\System\CurrentControlSet\Services`��һ���¡����Բ鿴��һ���ֱȽϼ򵥡�

�������ĵ�һ�����������ֹؼ����ݵĻ�ȡ��ֻ�л�ȡ��ע���ļ�ֵ��ָ���Ŀ������Ҳ��ܹ�����һ���ķ�����������������ҷ�����Щ����ָ���`DLL`�ļ���`Parameters`��һ�Ӽ��µ�`serviceDLL`ֵ��ָ���ģ�����Щ�����ǵ�ǰ����ļ��µ�`ImagePath`ֵ��ָ���ĵġ�����Ҫ����������������ǡ�

�����õ����ȼ���`serviceDLL`�����ȼ�Ҫ�ߣ������Լ��½���һ��������������Ҫ`ImagePath`��`Type`��`Start`������ֵ�Ĵ��ڣ�����ֵ�Ƕ��٣����ܹ���`Autoruns`��`Drivers`��һ����ʾ������ϵͳ������������`Parameters`��һ�Ӽ��µ�`serviceDLL`ֵ��ָ���ģ������Ծ���С��������`ImagePath`ָ����

�������ĵڶ��������Ǵ������`ImagePath`��ʹ���˻�������`SystemRoot`������Ҫ����ֵ�������п��ܳ��ֵĻ���������

�������ĵ�����������`Autoruns`��ÿһ�������������ע����е�����������ֵ��ָ���ģ��ֱ��ǵ�ǰ����ļ���`Description`��`DisplayName`����������ָ���ģ�������������ʹ�õĶ���[�ַ����ض���](https://stackoverflow.com/questions/22273956/how-to-get-redirected-string-from-the-registry)����΢���`Multilingual User Interface`֧�ֵı��ػ������͵������ǣ�

```
@%SystemRoot%\system32\shell32.dll,-21791
```

�Ұ���`StackOverflow`�ϵĴ����`advapi.dll`�еĺ�����װ����`c#`�ڣ���������ܹ��Զ��ؽ����������������ҵ�ָ�����ļ�����������ȡ���������dll����ϸ������

�������ĵ��ĸ������Ǵ�����ķ���dll���ǰ�֤����Ƕ�ڿ�ִ���ļ��еģ�����֮ǰ���ļ�֤������ȡ�����ߵķ������ܼ������ˡ�ѯ����ʦ���ҵ�֪����֤�鲻��Ƕ���ļ��е����˾�����Ч��ԭ����ʹ����`Catalog Signature`�ļ�������ϵͳĿ¼`C:\Windows\System32\catroot\{F750E6C3-38EE-11D1-85E5-00C04FC295EE}`���кܶ��`.cat`�ļ�����Щ�ļ��а������Ѿ�����֤�ĳ����sha1ֵ���൱����һ���ļ�������������ļ���

������Ѱ��һȦ��û�з���`C#`�п��õ�`API`��ʵ��������ܣ����ǵ�ʹ�ø߼���`C#`����`C++`������Ҫ���Ѹ����ʱ�䣬������һ�����Ҿ�û�����ȥ�ˡ�

###  Drivers��ϵͳ��������

ϵͳ���������Servicesϵͳ������ͬһ�����£�������û�ж�д�ܶ�Ĵ��룬ֻ�Ǽ򵥵İ��տ�ִ���ļ����Ƿ�����`.sys`��β��������`ϵͳ����������`����`ϵͳ��������������`��

###  Scheduled Tasks���ƻ�����

�ƻ�������`Windows`ϵͳ�Դ���һ������������û��Լ������Զ��������񡣳����ļƻ�������ǿ�����������

����`C#`����������һ��`.COM`��`taskScheduler`���ã�ʹ�����API��ʵ����Ӧ�Ĺ��ܡ����������Ͷ�ȡע������ƣ�ֻ�����ҽ�����ִ���ļ�·����ʱ����Ҫ����һ��XML��ʽ�����ݡ�

## ��ѡ�Ĺ���ʵ��

### Internet Explorer��IE ������� BHO ����

## ��¼

### Logonע���

```
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Userinit",
"HKLM\System\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\StartupPrograms",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\AppSetup",
"HKLM\Software\Policies\Microsoft\Windows\System\Scripts\Startup",
"HKCU\Software\Policies\Microsoft\Windows\System\Scripts\Logon",
"HKLM\Software\Policies\Microsoft\Windows\System\Scripts\Logon",
"HKCU\Environment\UserInitMprLogonScript",
"HKLM\Environment\UserInitMprLogonScript",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Userinit",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\VmApplet",
"HKLM\Software\Policies\Microsoft\Windows\System\Scripts\Shutdown",
"HKCU\Software\Policies\Microsoft\Windows\System\Scripts\Logoff",
"HKLM\Software\Policies\Microsoft\Windows\System\Scripts\Logoff",
"HKCU\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup",
"HKLM\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Startup",
"HKCU\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Logon",
"HKLM\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Logon",
"HKCU\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Logoff",
"HKLM\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Logoff",
"HKCU\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Shutdown",
"HKLM\Software\Microsoft\Windows\CurrentVersion\Group Policy\Scripts\Shutdown",
"HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System\Shell",
"HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Shell",
"HKLM\Software\Microsoft\Windows\CurrentVersion\Policies\System\Shell",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Shell",
"HKLM\SYSTEM\CurrentControlSet\Control\SafeBoot\AlternateShell",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\Taskman",
"HKLM\Software\Microsoft\Windows NT\CurrentVersion\Winlogon\AlternateShells\AvailableShells",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Runonce",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\RunonceEx",
"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Run",
"HKLM\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp\InitialProgram",
"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
"HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run",
"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
"HKCU\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Run",
"HKCU\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnceEx",
"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce",
"HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce",
"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce",
"HKCU\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\RunOnce",
"HKCU\Software\Microsoft\Windows NT\CurrentVersion\Windows\Load",
"HKCU\Software\Microsoft\Windows NT\CurrentVersion\Windows\Run",
"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run",
"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\Run",
"HKLM\SOFTWARE\Microsoft\Active Setup\Installed Components",
"HKLM\SOFTWARE\Wow6432Node\Microsoft\Active Setup\Installed Components",
"HKLM\Software\Microsoft\Windows NT\CurrentVersion\Windows\IconServiceLib",
"HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Runonce",
"HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\RunonceEx",
"HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\Install\Software\Microsoft\Windows\CurrentVersion\Run",
"HKLM\SOFTWARE\Microsoft\Windows CE Services\AutoStartOnConnect",
"HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows CE Services\AutoStartOnConnect",
"HKLM\SOFTWARE\Microsoft\Windows CE Services\AutoStartOnDisconnect",
"HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows CE Services\AutoStartOnDisconnect",
```
