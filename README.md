# capstone_dotnetframework (A C# .NET Framework middleware between SQL and Arduino)

## About

This is a school project using C# .NET Framework 4.7.2 and the Windows Presentation Foundation (WPF) UI subsystem. It is the middleware component of our capstone project entitled "AAMS: AUTOMATIC ATTENDANCE MONITORING SYSTEM". It interfaces between a Microsft SQL Server instance (not provided) and the serial output of an Arduino UNO (source not provided).

This is the last update of the source, dated May 8, 2023.

## Building

### Requirements

- Windows 11
- Visual Studio 2022
- .NET Framework 4.7.2
- Newtonsoft.Json

### How to Compile

1. Install Newtonsoft.Json through NuGet:
    - Restore NuGet packages in Visual Studio.
2. Compile through Visual Studio 2022 or MSVC.

## How to Use
1. Edit the config_template.json in the Template folder to your liking.
2. Copy it to where the .exe is located.
3. Run the capstone-dotnetframework.exe executable.

## License

This project uses the MIT license. [Learn more here.](https://choosealicense.com/licenses/mit/)
