#!/bin/bash
echo "change dir..."
cd ../src
echo "dir:`pwd`"

echo -e '\n'

echo "git pull repo from parent..."
git pull parent dev
echo "git pull repo from parent complated!"

echo -e '\n'

echo "git commit repo into local..."
git add -A
git commit -m "updated at:$(date '+%Y-%m-%d %H:%M:%S')"
echo "git commit repo into local complated!"

echo -e '\n'

echo "git push repo to origin...!"
git push origin dev
echo "git push repo to origin complated!"
