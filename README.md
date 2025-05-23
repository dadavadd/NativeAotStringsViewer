# NativeAotStringsViewer

**NativeAotStringsViewer** is a low-level diagnostic tool for analyzing and extracting managed strings from NativeAOT-compiled .NET binaries.
It uses memory inspection and metadata parsing techniques to locate and decode string objects from the *FrozenObjectRegion* section of ReadyToRun (R2R) modules.

### Features
* Supports both x86 and x64 architecture layouts.
* Automatically identifies and parses string structures based on VTable matching.
* Escapes and presents strings in a safe, readable format.
* Useful for reverse engineering, debugging, or analyzing AOT-compiled .NET applications.
