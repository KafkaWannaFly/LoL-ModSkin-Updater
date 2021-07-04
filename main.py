import urllib.request
import requests
import zipfile


def getDownloadURL():
    response = urllib.request.urlopen(
        "http://modskinpro.com/p/tai-phan-mem-mod-skin-lol-pro-2020-chn").read()

    contents = f'{response}'

    linkBeginIdx = contents.find('link3 = "', 0, len(contents))
    linkEndIdx = contents.find('";', linkBeginIdx, len(contents))

    dowloadLink = contents[linkBeginIdx+len('link3 = "'): linkEndIdx]

    print(f'Get download link: {dowloadLink}')
    return dowloadLink


def downloadFile(fileUrl):
    r = requests.get(fileUrl)

    filename = fileUrl[fileUrl.rfind('/') + 1: len(fileUrl)]
    print(f'Write to {filename}')

    open(filename, 'wb', ).write(r.content)

    return filename


def extractFile(filename, outDir):
    with zipfile.ZipFile(filename, 'r') as zip_ref:
        zip_ref.extractall(outDir)

    print(f'Extract file to ${outDir}')


if __name__ == '__main__':
    input('Ready download new version of LOL Modskin. Press enter to proceed...')

    fileName = downloadFile(getDownloadURL())

    extractFile(fileName, 'Modskin')
