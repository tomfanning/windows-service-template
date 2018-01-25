#define MyAppPublisher "MyCo Limited"
#define MyAppURL "http://www.myco.co.uk"
#define MyAppExeName "windows-service.exe"
#define ApplicationVersion GetFileVersion("windows-service.exe")
#define MyAppName "Windows Service"
#define OutputName "windows-service"
#define VariantUUID "FEEC8DE1BCAC42D083BEC07835656D95"

[CustomMessages]
DependenciesDir=MyProgramDependencies

[Setup]
AppID={#VariantUUID}
AppName={#MyAppName}
AppVersion={#ApplicationVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\MyCo\{#MyAppName}
DefaultGroupName={#MyAppName}
AppCopyright=Copyright {#MyAppPublisher}
OutputBaseFilename="{#OutputName}-{#ApplicationVersion}"
Compression=lzma2/normal
LZMANumBlockThreads=2
VersionInfoVersion={#ApplicationVersion}
OutputDir=.
InfoBeforeFile=infobefore.rtf

; Require Admin rights for the install
PrivilegesRequired=admin
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
RestartIfNeededByRun=no

WizardSmallImageFile=installer-logo.bmp
WizardImageFile=compiler:wizmodernimage-is.bmp

Uninstallable=no

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Tasks]

[Dirs]
Name: "{app}\logs"; Flags: setntfscompression

[Files]
Source: {#MyAppExeName}; DestDir: {app}; Flags: ignoreversion
Source: {#MyAppExeName}.config; DestDir: {app}; Flags: ignoreversion

Source: *.dll; DestDir: {app}; Flags: ignoreversion
Source: *.pdb; DestDir: {app}; Flags: ignoreversion

[Icons]

[Run]
Filename: {app}\{#MyAppExeName}; Parameters: "--install"
Filename: net.exe; Parameters: "start servicename"; Description: "Start service now"; Flags: postinstall runascurrentuser runhidden

[UninstallRun]
Filename: {app}\{#MyAppExeName}; Parameters: "--uninstall"

[Code]

[Registry]
