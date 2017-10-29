# UltimateAdmin

UltimateAdmin is an application built for system administrators to query Windows machines in Active Directory for the purposes of machine management.  While some of this information can be readily obtained from the Active Directory Users & Computers MMC snap-in, this tool compiles the information together in a format that is easier for managing a specific device.  

Users of the application should have an administrative account in an Active Directory domain with admin rights on the target computers to use the application as intended.  Upon launching the application, it will prompt the user to run it in an elevated state.  The benefit to this is that users of the application can be logged on as a standard user so that only functions related to system administration are run in the "sandbox" of the app.  This provides a separation of concerns in terms of the admin's responsibilities.

The application can be launched from a machine that is not part of an Active Directory domain, and it will run in DEMO mode to showcase how the basic functionality works.  When running it outside an AD domain, you will see the below message, after which the app will run in DEMO mode:

![AD_Error.jpg]({{site.baseurl}}/AD_Error.jpg)


##Features
---
1. Search Active Directory using a wildcard search, targeting either machine name or description keywords.  

2. From the search, double-click to open the machine to obtain detailed information including online status, timestamped at last refresh.

1. Configure custom groups to query only specified organizational units for faster querying in very large domains or slow networks.

2. Query logged in user.

4. Update computer description in AD.

5. Obtain Bit Locker Recovery Key if available.

6. Query security group memberships

6. Automatic querying of detailed system information such as:
	- Type: Desktop/Laptop
    - BIOS: Current level
    - Make/Model: Brand, model information
    - Serial: Machine serial number
    - OS: Current Operating System
    
8. Manage local administrators and remote desktop users on the target machine.

9. Access the machine from Microsoft's built-in remote access tools: Remote Desktop and Remote Assist.

10. Remote shutdown and restart.

11. Access remote machine using alternative to PSEXEC, PAEXEC: [https://github.com/poweradminllc/PAExec](https://github.com/poweradminllc/PAExec)

12. Quickly access built-in tools such as Active Directory Users and Computers, Computer Management, Registry Editor, PAExec, and the Command Line all running as administrator.

Below is a screenshot of the app in demo mode:

![MainWindow.jpg]({{site.baseurl}}/MainWindow.jpg)

