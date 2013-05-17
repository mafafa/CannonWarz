[Setup]
AppName=Cannon Warz
AppVersion=1.1
AppPublisher=Mathieu-Alexandre Fafard
DefaultDirName={pf32}\Cannon Warz
DefaultGroupName=Cannon Warz
UninstallDisplayIcon={app}\Cannon Warz.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.\Installation
OutputBaseFilename=Cannon Warz Setup

AlwaysRestart=yes
ArchitecturesAllowed=x86 x64
CreateAppDir=yes
Uninstallable=yes
CreateUninstallRegKey=yes

[Icons]
Name: "{commondesktop}\Cannon Warz"; Filename: "{app}\Cannon Warz.exe"; WorkingDir: "{app}"

[Files]
; Game Files
Source: ".\CannonWarz\bin\x86\Release\Content\*"; DestDir: "{app}\Content"; Flags: ignoreversion
Source: ".\CannonWarz\bin\x86\Release\Cannon Warz.exe"; DestDir: "{app}"; Flags: ignoreversion

; Dependencies
Source: ".\Installation\Include\dotNetFx40_Client_x86_x64.exe"; DestDir: "{app}"; Flags: ignoreversion deleteafterinstall
Source: ".\Installation\Include\xnafx40_redist.msi"; DestDir: "{app}"; Flags: ignoreversion deleteafterinstall 

[Run]
Filename: "{app}\dotNetFx40_Client_x86_x64.exe"; Description: "Installing Microsoft .Net 4"; Parameters: "/q /norestart"; Check: IsDotNetInstalled
Filename: "{app}\xnafx40_redist.msi"; Description: "Installing XNA Redist"; Parameters: "/nologo /quiet"; Check: IsXNARedistInstalled

[Code]
function IsDotNetInstalled(): Boolean;
begin
  if RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client') = False then
    Result := True
  if RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client') = True then
    Result := False
end;

function IsXNARedistInstalled(): Boolean;
begin
  if RegKeyExists(HKLM, 'SOFTWARE\Wow6432Node\Microsoft\XNA\Framework\v4.0') = False then
    Result := True
  if RegKeyExists(HKLM, 'SOFTWARE\Wow6432Node\Microsoft\XNA\Framework\v4.0') = True then
    Result := False
end;