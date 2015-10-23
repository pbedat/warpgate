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
  
Copy
----
So you have launched a warpgate? Ok! It's time pewpewpew:

  ./warpgate.exe cp <url> <path>
  
when everything went allright you should see the warp location.
