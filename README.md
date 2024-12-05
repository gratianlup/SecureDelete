SecureDelete
============

A feature-rich application for securely deleting files and even entire drives.  
Notable is the inclusion of a partial implementation of the NTFS file system, used to remove all traces of the files.  

Main functionality written in *C++*, extended by a library written in *C#* with features like file filtering, scheduling and plug-ins, and a software interface build using *WinForms* available as a stand-alone application, *Windows Shell* extension and as a *Microsoft Management Console* plug-in

### Main features:  
- Wiping of files, folders and free space on a disk.
- Wiping of cluster tips, Alternate Data Streams and MFT file entries (under the NTFS file system).
- Built-in and user-defined wipe methods supporting patterns, random data and write verification.
- Secure random number generators (ISAAC and Mersenne Twister) and seed generator.
- Parallel wiping for targets found on separate physical disks.
- Scheduling of wiping tasks with custom options and before/after actions.
- Plug-in framework allows extending the existing functionality.
- Actions that can be executed at the beginning/end of the wiping process.
- PowerShell scripts can be used to controll the wiping process or scheduled tasks.
- Windows Shell integration (context menu entries in Windows Explorer, Open/Save dialogs, etc.)
- Microsoft Management Console integration.
- Rich reporting, logging, debugging and crash protection capabilities.
- Exporting and importing of options, wipe methods and scheduled tasks.

For a complete list of features and some implementation details download the following document:  

[Features and implementation (PDF)](https://github.com/user-attachments/files/18029653/secure_delete.pdf)

### Component roles:
**SecureDeleteBridgeExample**: An example of a PowerSheel Action bridge object.  
**SecureDeleteClient**: The main application hosting the GUI.  
**SecureDeleteCrashReporter**: Reports information about the application crash to the developer.  
**SecureDeleteDebugger**: Receives and displays debugging information from the application.  
**SecureDeleteFileStore**: Implements a "file-system in a file", used to store various objects and settings.  
**SecureDeleteFileStoreBrowser**: Viewer and editor for FileStore-generated files.  
**SecureDeleteMMC**: Exposes the GUI as a Microsoft Management Console plug-in.  
**SecureDeleteManaged**: Exposes the functionality to .NET applications and adds features like file searching and filtering, sessions,  
actions, scheduling, reporting, a plug-in framework and exporting/importing settings, wipe methods and scheduled tasks.  
**SecureDeleteNative**: Implements the main wiping functionality.  
**SecureDeletePluginExample**: An example of a wipe plug-in.    
**SecureDeletePluginWizard**: A wizard for creating wipe plug-ins that can be integrated with Visual Studio.  
**SecureDeleteShellExtensionClient**: Implements the GUI used when the application is invoked from the Windows Shell.  
**SecureDeleteShellExtensionNative**: Implements a Windows Shell Extension that adds wipe functionality to the context menus.  
**SecureDeleteTextParser**: Parses a text based on some predefined rules.    
**SecureDeleteWinForms**: Implements the GUI for all modules of the application.  
**SecureDeleteWindowsMessageListener**: A listener which sends messages to the debugger.    
