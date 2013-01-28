SecureDelete
============



The following documents describe the architecture in more detail:  

**[Download document describing the features and their implementation (PDF)](http://www.gratianlup.com/documents/secure_delete_documentation.pdf)**  
**[Download diagram for the SecureDeleteManaged component (PDF)](http://www.gratianlup.com/documents/secure_delete_managed_diagram.pdf)**  
**[Download diagram for the SecureDeleteNative component (PNG)](http://www.gratianlup.com/documents/secure_delete_native_diagram.png)**  

### Architecture overview:

![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_architecture.PNG)  


### Component roles:
**SecureDeleteBridgeExample**: An example of a PowerSheel Action bridge object.  
**SecureDeleteClient**: The main application hosting the GUI.  
**SecureDeleteCrashReporter**: Reports information about the application crash to the developer.  
**SecureDeleteDebugger**: Receives and displays debugging information from the application.  
**SecureDeleteFileStore**: Implements a "file-system in a file", used to store various objects and settings.  
**SecureDeleteFileStoreBrowser**: Viewer and editor for FileStore-generated files.  
**SecureDeleteMMC**: Exposes the GUI as a Microsoft Management Console plug-in.  
**SecureDeleteManaged**: Exposes the functionality to .NET applications and adds features like file searching and filtering, sessions,  
actions, scheduling, reporting and a plug-in framework.  
**SecureDeleteNative**: Implements the main wiping functionality.  
**SecureDeletePluginExample**: An example of a wipe plug-in.    
**SecureDeletePluginWizard**: A wizard for creating wipe plug-ins that can be integrated with Visual Studio.  
**SecureDeleteShellExtensionClient**: Implements the GUI used when the application is invoked from the Windows Shell.  
**SecureDeleteShellExtensionNative**: Implements a Windows Shell Extension that adds wipe functionality to the context menus.  
**SecureDeleteTextParser**: Parses a text based on some predefined rules.    
**SecureDeleteWinForms**: Implements the GUI for all modules of the application.  
**SecureDeleteWindowsMessageListener**: A listener which sends messages to the debugger.    

### Screenshots:

Main window (adding a folder)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_folder.PNG)  

Main window (adding a folder using advanced filters)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_folder_filters.PNG)  

Scheduled wipe task options (execution time configuration)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_schedule_options.PNG)  

Scheduled wipe task options (actions to run after execution)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_schedule_action_custom.PNG)  

PowerShell action editor (can be run after/before a wipe task)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_schedule_action_powershell.PNG)  

Options window  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_options_general.PNG)  

Options window (schedulling options)  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_options_scheduling.PNG)  

Import/export settings window  
![SmartFlip screenshot](http://www.gratianlup.com/documents/secure_delete_export.PNG)  
