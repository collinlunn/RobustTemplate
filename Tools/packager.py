#!/usr/bin/env python3
# Packages a full release build that can be unzipped and you'll have your client or server.

import os
import shutil
import subprocess
import sys
import zipfile
import argparse
from typing import List
from zipfile import ZipFile

p = os.path.join

class PlatformReg:
    def __init__(self, rid: str, target_os: str, build_by_default: bool):
        self.rid = rid
        self.target_os = target_os
        self.build_by_default = build_by_default

PLATFORMS = [
    PlatformReg("win-x64", "Windows", True),
    PlatformReg("linux-x64", "Linux", False),
    PlatformReg("linux-arm64", "Linux", False),
    PlatformReg("osx-x64", "MacOS", False),
    # Non-default platforms (i.e. for Watchdog Git)
    PlatformReg("win-x86", "Windows", False),
    PlatformReg("linux-x86", "Linux", False),
    PlatformReg("linux-arm", "Linux", False),
]

PLATFORM_RIDS = {x.rid for x in PLATFORMS}
PLATFORM_RIDS_DEFAULT = {x.rid for x in filter(lambda val: val.build_by_default, PLATFORMS)}

SERVER_BIN_SKIP_FOLDERS = [
    # Roslyn localization files, screw em.
    "cs",
    "de",
    "es",
    "fr",
    "it",
    "ja",
    "ko",
    "pl",
    "pt-BR",
    "ru",
    "tr",
    "zh-Hans",
    "zh-Hant"
]

CLIENT_BIN_SKIP_FOLDERS = [
]

SERVER_RES_SKIP_FOLDERS = [
    #Content
    "Textures",
    "Fonts",
    "Audio",
    "Shaders",
    "Fonts",
    "keybinds.yml",

    #Engine
    "EngineFonts",
]

CLIENT_RES_SKIP_FOLDERS = [
    "Commands",
]

SERVER_RES_SKIP_FILES = [
    ".gitignore",
]

CLIENT_RES_SKIP_FILES = [
    ".gitignore",
]

SERVER_CONTENT_ASSEMBLIES = [
    "Content.Server",
    "Content.Shared",
]

CLIENT_CONTENT_ASSEMBLIES = [
    "Content.Client",
    "Content.Shared",
]

def main() -> None:
    (platforms, standalone, hybrid_acz) = parse_args()

    if not platforms:
        platforms = PLATFORM_RIDS_DEFAULT

    wipe_bin()
    clean_release_folder()

    for platform in PLATFORMS:
        rid = platform.rid

        if rid not in platforms:
            continue

        publish_project("../RobustToolbox/Robust.Client/Robust.Client.csproj", rid)
        publish_project("../Content.Client/Content.Client.csproj", rid)
        
        publish_project("../RobustToolbox/Robust.Server/Robust.Server.csproj", rid)
        publish_project("../Content.Server/Content.Server.csproj", rid)

        if standalone:
            package_client_standalone(rid)
        if hybrid_acz:
            package_client_acz(rid)
        package_server(rid)

    wipe_bin()

def parse_args() -> (str, bool, bool):
    parser = argparse.ArgumentParser(
        description="Packages the content repo for release on all platforms.")
    parser.add_argument("--platform",
        "-p",
        action="store",
        choices=PLATFORM_RIDS,
        nargs="*",
        help="Which platform to build for. If not provided, all platforms will be built.")
    parser.add_argument("--standalone",
        action="store_false",
        help="Packages a fully executable client zip.")
    parser.add_argument("--hybrid-acz",
        action="store_false",
        help="Packages a partial client zip with only non-engine resources and assemblies.")
    
    args = parser.parse_args()
    return (args.platform, args.standalone, args.hybrid_acz)  

def clean_release_folder() -> None:
    if os.path.exists("release"):
        print("Cleaning old release packages...")
        shutil.rmtree("release")
    os.mkdir("release")

def wipe_bin():
    print("Clearing old build artifacts (if any)...")

    if os.path.exists("../RobustToolbox/bin"):
        shutil.rmtree("../RobustToolbox/bin")

    if os.path.exists("../bin"):
        shutil.rmtree("../bin")

def publish_project(project: str, runtime: str) -> None:
    print(f"Publishing project for {runtime}...")
    subprocess.run(["dotnet", "publish",
        project,
        "--runtime", runtime,
        "--no-self-contained",
        "-c", "Release",
        "--nologo",
        "-v", "m",
        "-p", "FullRelease=True"
        ], check=True)

def make_zip(name: str) -> ZipFile:
    return ZipFile(
        f"release/{name}.zip", 
        "w",
        compression=zipfile.ZIP_DEFLATED)

def package_server(runtime: str) -> None:
    print(f"Packaging {runtime} server...")
    server_zip = make_zip(f"RobustTemplate.Server_{runtime}")
    copy_dir_into_zip(f"../RobustToolbox/bin/Server/{runtime}/publish", ".", server_zip, SERVER_BIN_SKIP_FOLDERS)
    copy_content_assemblies(f"../bin/Content.Server/{runtime}/publish", "Resources/Assemblies", SERVER_CONTENT_ASSEMBLIES, server_zip)
    copy_dir_into_zip("../Resources", "Resources", server_zip, SERVER_RES_SKIP_FOLDERS, SERVER_RES_SKIP_FILES)
    copy_dir_into_zip("../RobustToolbox/Resources", "Resources", server_zip, SERVER_RES_SKIP_FOLDERS, SERVER_RES_SKIP_FILES)
    server_zip.close()

def package_client_standalone(runtime: str) -> None:
    print(f"Packaging {runtime} standalone client...")
    client_zip = make_zip(f"RobustTemplate.Client_Standalone_{runtime}")
    copy_dir_into_zip(f"../RobustToolbox/bin/Client/{runtime}/publish", ".", client_zip, CLIENT_BIN_SKIP_FOLDERS)
    copy_content_assemblies(f"../bin/Content.Client/{runtime}/publish", "Resources/Assemblies", CLIENT_CONTENT_ASSEMBLIES, client_zip)
    copy_dir_into_zip("../Resources", "Resources", client_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
    copy_dir_into_zip("../RobustToolbox/Resources", "Resources", client_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)    
    client_zip.close()

def package_client_acz(runtime: str) -> None:
    print(f"Packaging {runtime} acz client...")
    acz_zip = make_zip(f"RobustTemplate.Client_ACZ_{runtime}")
    copy_content_assemblies(f"../bin/Content.Client/{runtime}/publish", "Assemblies", CLIENT_CONTENT_ASSEMBLIES, acz_zip)
    copy_dir_into_zip("../Resources", ".", acz_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
    copy_dir_into_zip("../RobustToolbox/Resources", ".", acz_zip, CLIENT_RES_SKIP_FOLDERS, CLIENT_RES_SKIP_FILES)
    acz_zip.close() 

def copy_dir_into_zip(directory: str, target: str, zipf: ZipFile, skip_folders: List[str]={}, skip_files: List[str]={}):
    for root, dirnames, files in os.walk(directory):
        relpath = os.path.relpath(root, directory)
        if relpath in skip_folders:
            dirnames.clear() #prevents sub-directories of skipped folder from getting copied
            continue

        if relpath != ".":
            zipf.write(root, p(target, relpath))

        for filename in files:
            if filename in skip_files:
                continue
            zippath = p(target, relpath, filename)
            filepath = p(root, filename)
            zipf.write(filepath, zippath)

def copy_content_assemblies(directory: str, target: str, assemblies: List[str], zipf: ZipFile):
    fileNames = []

    for assembly in assemblies:
        fileNames.append(assembly + ".dll")
        fileNames.append(assembly + ".pdb")

    zipf.write(".", target)

    for fileName in fileNames:
        zipf.write(f"{directory}/{fileName}", p(target, fileName))

if __name__ == '__main__':
    main()