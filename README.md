# Pluggr – gRPC-Powered Process Communication Framework
Pluggr started as another experimental project designed to allow me (or other developers) to build software components and combine them into a single interface. This used DLL injection and had a dedicated C# WPF-based main interface (the 'Application Host').

However, this evolved through iterations into this current iteration on the idea called Pluggr. Which relies on a Pluggr Server to act as the middle-man between services (called Pluggr Clients in the code). Which can register themselves with the Server and be used be other Pluggr applications to send data back-and-forth between them. Now using gRPC as the technology powering this inter-process communication.

This framework is designed for some upcoming projects that I am working on and is _not_ ready for serious commercial projects. I enjoy working on this project, though, and plan to use it for more personal software in the future.

## Demo
To see Pluggr in action, check out the [1.0 demo](https://github.com/MicahClarkeR/Pluggr/releases/tag/1.0). This is a basic demo of the framework with two console-based applications. One will act as the service being ran on Pluggr. The other has a basic input that will forward user input onto this service, which will then be returned back. It is a basic demo designed to show how Pluggr process communication works.

<img width="1408" height="637" alt="image" src="https://github.com/user-attachments/assets/aef5c947-084e-4591-a30d-e438b65cde08" />
<em>An example showing a message being gathered from the user, sent to the service, and returned. With the service logging the message.</em>
