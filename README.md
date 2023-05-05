
# Minimum Repro Example 

Please forgive me if I've made some noob mistakes - I'm relatively new to C#. I've had trouble getting nuget packages to play ball and have had to do all sorts of strange things to get DLLs visible for this example. Initially I tried using the latest versions of all packages but following some discussions on various forums I tried to downgrade some of them (EntityFrameworkCore). Any assistance in this would be greatly appreciated!

## Expected Behaviour
- It should be possible to use the EntityFramework to create a SQL database if it does not exist, and to interact with said database.

## Current Behaviour
- Receiving the error `Microsoft.Data.SqlClient is not supported on this platform`
- I am running Windows 11 and ManicTime is written using .NET.

## My dotnet --info

```
PS> dotnet --info
.NET SDK:
 Version:   7.0.203
 Commit:    5b005c19f5

Runtime Environment:
 OS Name:     Windows
 OS Version:  10.0.22621
 OS Platform: Windows
 RID:         win10-x64
 Base Path:   C:\Program Files\dotnet\sdk\7.0.203\

Host:
  Version:      7.0.5
  Architecture: x64
  Commit:       8042d61b17

.NET SDKs installed:
  6.0.408 [C:\Program Files\dotnet\sdk]
  7.0.203 [C:\Program Files\dotnet\sdk]

.NET runtimes installed:
  Microsoft.AspNetCore.App 6.0.16 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.AspNetCore.App 7.0.5 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
  Microsoft.NETCore.App 6.0.15 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.NETCore.App 6.0.16 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.NETCore.App 7.0.5 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
  Microsoft.WindowsDesktop.App 6.0.15 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
  Microsoft.WindowsDesktop.App 6.0.16 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
  Microsoft.WindowsDesktop.App 7.0.5 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
```

## Instructions

1. Clone this repo and open the solution in Visual Studio.
2. Install ManicTime Free version (https://www.manictime.com/download/).
3. Ensure that your preferred local MS SQL Server instance is installed and running.
4. Open this solution and update the connection string in `TagPlugin.Data.Context` to reflect a valid connection string to your database with rights to allow for DDL statement execution.
5. Open a File Explorer and navigate to `%APPDATA%\..\Local\Finkit\ManicTime\Plugins` - this is where the build must be copied to so that the ManicTimeClient.exe can read it.
6. Ensure that the appropriate nuget packages have been installed on your system, namely:
	```
	microsoft.data.sqlclient\5.1.1
	microsoft.entityframeworkcore\6.0.16
	microsoft.entityframeworkcore.abstractions\6.0.16
	microsoft.entityframeworkcore.relational\6.0.16
	microsoft.entityframeworkcore.sqlserver\6.0.16
	microsoft.extensions.caching.abstractions\6.0.0
	microsoft.extensions.caching.memory\6.0.1
	```
	I also installed the latest version of EntityFrameworkCore, but this configuration is the one that replicates my current state.
7. Update the relative paths for the required assemblies (sorry!) in: 
	- `TagPlugin.Data.csproj` for `Microsoft.EntityFrameworkCore` and `Microsoft.EntityFrameworkCore.SqlServer`
	- `TagPlugin.Example.csproj` for `Microsoft.Data.SqlClient`, `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Abstractions`, `Microsoft.EntityFrameworkCore.Relational`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.Extensions.Caching.Abstractions`, and `Microsoft.Extensions.Caching.Memory`

8. In the Post Build Events - Visual Studio should copy the resulting plugin build to the appropriate directory.
9. Close and restart ManicTime (it needs to be closed from the taskbar and may be a hidden icon).
10. In the options menu at the top-right of the ManicTime screen, the plugins can be accessed, find and activate this sample plugin.
11. The `Send tags` command probably won't produce any output, but if it's working correctly it should create a text file and inform you of this with a MsgBox popup.
12. Try to create the database.
13. Open the application log of ManicTime, found in `%APPDATA%\..\Local\Finkit\ManicTime\Logs` and check most recent entries for any errors. If there are none, open SSMS and check whether the database was created.
14. Please please PLEASE let me know if it worked for you.


--------------------------------------------------------------------------------------------


Client plugins for ManicTime
===============================

An example of how to create custom ManicTime client plugins. ManicTime client supports three different types of plugins: 
- Tag source plugin
- Timeline plugin
- Tracker plugin

Tag Plugin
====================

Within ManicTime a tag plugin can be used to:
- Get tags to ManicTime from an outside service
- Export created tag activities (tags with start and end time) to an outside service

There are two important files:
ImportTags/TagsImporter.cs
Here you can get your tags from any service and pass them to ManicTime as a collection of TagSourceItem objects.

ExportTags/TagsExporter.cs
This will return a collection of TagActivity objects which you can then export to other services.


How to use
----------

1. Compile the project (source/tag-plugin).
2. After you compile it, there should be a folder in repository root - installable-plugin/< BuildConfiguration >
3. Go to ManicTime, Settings -> Advanced -> Open db folder
4. Copy folder installable-plugin/< BuildConfiguration >/Plugin to database folder, so that in the database folder it looks like
....\db folder\Plugins\Packages\ManicTime.TagSource.SampleTagPlugin\...
5. Run ManicTime

If you go to Plugins, you should now see a new plugin

![Find Tag plugin](http://manictimecdn.blob.core.windows.net/images/github/tag-plugin-installed.png)

Click on Settings, to add new connection

![Add new connection](http://manictimecdn.blob.core.windows.net/images/github/tag-plugin-settings.png)

Then go to Add tag, you should see some new tags

![Create a tag](http://manictimecdn.blob.core.windows.net/images/github/tag-plugin-imported-tags.png)

Make some tags, then export them

![Export tags](http://manictimecdn.blob.core.windows.net/images/github/tag-plugin-export-tags.png)


Debugging in ManicTime
===============

Client plugins (Tag plugin and Timeline plugin) can be difficult to troubleshoot or simply irritating to test manually.

One thing you can do, is attach debugger to ManicTimeClient.exe process.

To do this:
1. Switch to Debug build.
2. Open plugin project properties.
3. Under Build property, "Output" section: edit "Output path:" relative to ManicTime's database folder. e.g. [ManicTime Database folder]\Plugins\Packages\ < PackageName >\Lib\
3. Under Debug property, "Start action" section: pick "Start external program:" and set it to ManicTimeClient.exe executable (ManicTime.exe in case of TrackerPlugin). e.g. C:\Program Files (x86)\ManicTime\ManicTimeClient.exe
4. Make sure ManicTime Client settings option "Settings -> General -> Keep user interface running when main window closes" is unchecked.
5. Make sure ManicTime Client is not running.
6. Press Start (F5) to start debugging

Starting project in Debug mode should open instance of ManicTime Client with debugger attached. 
Any breakpoints set in project should now be hit as the plugin code executes.


