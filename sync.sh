#!/bin/bash
cd ../src
git pull parent dev
git add -A
git commit -m "updated at:$(date '+%Y-%m-%d %H:%M:%S')"
git push origin dev
