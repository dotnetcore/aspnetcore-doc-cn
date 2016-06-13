#!/bin/bash
cd ../src
git pull parent dev
git add -A
git commit -m "updated at:$(date +%Y%m%d)"
git push origin dev
