import os 
import os.path 
import shutil 
import time,  datetime

def copyFiles(sourceDir,  targetDir):
	count = 0
	for file in os.listdir(sourceDir):
		sourceFile = os.path.join(sourceDir,  file)
		targetFile = os.path.join(targetDir,  file)
		if os.path.isfile(sourceFile) and sourceFile.find('.json.txt') > 0:
			open(targetFile, "wb").write(open(sourceFile, "rb").read())
			count+=1
	print '  %d files copied.' % count
	
def duplicateFile(sourceFile,  startIndex, count):
	sourceFile = os.path.join(os.path.abspath('.'), sourceFile)
	for i in range(startIndex, startIndex + count):
		targetFile = os.path.join(os.path.abspath('.'), "%d.prefab"%i)
		open(targetFile, "wb").write(open(sourceFile, "rb").read())

# operations begin
targetDirs = [os.path.abspath('.')]
sourceDir = os.path.abspath('.')

print "\n>>>begin !\n"

#for path in targetDirs:
#	copyFiles(os.curdir, os.path.join(sourceDir, path))

duplicateFile("0.prefab", 1, 99)
	
print "\n>>>done !\n"
	









