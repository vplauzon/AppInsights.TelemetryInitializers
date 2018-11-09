#!/usr/bin/python3

import sys

def readAll(path):
    with open(path, 'r') as f:
        content = f.read()

        return content

def writeAll(path, content):
    with open(path, 'w') as f:
        f.write(content)

if len(sys.argv) != 3:
    print("There are %d arguments" % (len(sys.argv)-1))
    print("Arguments should be")
    print("1-File path")
    print("2-Patch number")
else:
    path = sys.argv[1]
    patchNumber = sys.argv[2]

    print ('Text file:  %s' % (path))
    print ('Patch Number:  %s' % (patchNumber))

    txtContent = readAll(path)
    
    print('Text content:')
    print(txtContent)

    alteredContent = txtContent.replace('.0</Version>', '.'+patchNumber+'</Version>')

    print('Altered content:')
    print(alteredContent)

    writeAll(path, alteredContent)