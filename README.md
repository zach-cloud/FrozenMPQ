Project is in progress.

# Background

While developing a tool to edit JASS code files, I encountered the need to extract and import JASS files from MPQ archives. The only existing Java project which can do this is JMPQ3, however it rebuilds the entire MPQ in order to add files, which does not work for my use case. It's open source, so I do eventually want to add this functionality to the JMPQ3 project.

In the meantime, I wanted a way to add this feature quickly. Existing MPQ libraries such as stormlibsharp exist, so I decided to make a very simple C# project which can be interfaced with on a command line, which will then by executed by the Java code. This was why I created FrozenMPQ

# Usage

# Build information
