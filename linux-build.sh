#!/bin/sh

# Get GitVersion
# Linux dependencies:
# $ sudo apt-get install mono-complete
# $ sudo apt-get install libcurl3
mkdir tmp 2>/dev/null
mkdir tmp/GitVersion 2>/dev/null
wget --no-clobber https://github.com/GitTools/GitVersion/releases/download/v4.0.0-beta.13/GitVersion_4.0.0-beta0013.zip --output-document=tmp/GitVersion.zip
unzip -u tmp/GitVersion.zip -d tmp/GitVersion

# Deal with a bug
#echo '<configuration><dllmap os="linux" cpu="x86-64" wordsize="64" dll="git2-baa87df" target="/usr/lib/x86_64-linux-gnu/libgit2.so.24" /></configuration>' > tmp/GitVersion/LibGit2Sharp.dll.config

mono tmp/GitVersion/GitVersion.exe

