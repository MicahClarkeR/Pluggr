# Pluggr – gRPC-Powered Process Communication Framework
Pluggr started as another experimental project designed to allow me (or other developers) to build software components and combine them into a single interface. This used DLL injection and had a dedicated C# WPF-based main interface (the 'Application Host').

However, this evolved through iterations into this current iteration on the idea called Pluggr. Which relies on a Pluggr Server to act as the middle-man between services (called Pluggr Clients in the code). Which can register themselves with the Server and be used be other Pluggr applications to send data back-and-forth between them. Now using gRPC as the technology powering this inter-process communication.

This framework is designed for some upcoming projects that I am working on and is _not_ ready for serious commercial projects. I enjoy working on this project, though, and plan to use it for more personal software in the future.
