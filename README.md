warpgate
========
A simple file http file transmission toolkit.

Why warpgate?
-------------
I often have to work with windows machine in foreign networks without the privileges to open other ports than 80 or 443.
Deploying software to hazardous environments like this is a huge pain in the ass.

So?
---
I want to create a toolkit capable of easing my deployment woes.

Features
========

Host
----
Before you begin to warp file from one place to another, you have to host.

    ./warpgate.exe host [--port | -p ]
  
Files sent by the warpgate will be saved to the current working dir.
  
Copy
----
So you have launched a warpgate? Ok! It's time pewpewpew:

    ./warpgate.exe cp <url> <path>
  
when everything went allright you should see the warp locations.

Relay
-----
Ok now sending files from one warpgate to another is easy. But what if a solar storm, wormhole or a firewall is blocking your http port? The solution is easy: Add another warpgate!

Setup a warpgate which relays your files to other warpgates on a machine, where you have open ports:

    ./warpgate.exe relay [--port | -p]
    
Setup a warpgate on the target machine and link it to the relay.

    ./warpgate.exe link <url>
    
Now use the uid provided by the linking warpgate to send files over the relay:

    ./warpgate.exe cp <url>/<uid> <path>

TODO
====

- Add CI
- Add Chocolaty Nuget integration
- Add apt integration
