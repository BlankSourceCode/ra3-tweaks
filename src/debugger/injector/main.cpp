#define _CRT_SECURE_NO_WARNINGS 1

#include <Windows.h>
#include <TlHelp32.h>
#include <stdio.h>

#define DLL_NAME L"dynity.dll"
#define PROC_NAME L"RobotArena3.exe"

static HANDLE GetProcessByName(wchar_t* name) 
{
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    PROCESSENTRY32 entry = { sizeof(PROCESSENTRY32) };
    size_t len = wcslen(name);
    Process32First(snapshot, &entry);
    do 
    {
        if (_wcsnicmp(name, entry.szExeFile, len) == 0) 
        {
            return OpenProcess(PROCESS_ALL_ACCESS, false, entry.th32ProcessID);
        }
    } while (Process32Next(snapshot, &entry));
    return 0;
}

int wmain(int argc, wchar_t *argv[], wchar_t *envp[])
{
    HANDLE proc = 0;
    HANDLE procThread = 0;
    bool notified = false;
    while (proc == 0) 
    {
        proc = GetProcessByName(PROC_NAME);
        if (proc == 0) 
        {
            if (!notified) 
            {
                printf("Waiting for RobotArena3.exe to start... gle=%x\n", GetLastError());
                notified = true;
            }
            Sleep(1);
        }
    }

    void *paramAddr = VirtualAllocEx(proc, 0, 0x1000, MEM_COMMIT, PAGE_READWRITE);
    SIZE_T bytesWritten = 0;
    wchar_t dllPath[MAX_PATH];
    int len = GetCurrentDirectory(MAX_PATH, dllPath);
    wcscpy(dllPath + len, L"\\" DLL_NAME);

    WriteProcessMemory(proc, paramAddr, dllPath, 0x100, &bytesWritten);
    HANDLE thread = CreateRemoteThread(proc, NULL, 0, (LPTHREAD_START_ROUTINE)LoadLibrary, (LPVOID)paramAddr, 0, 0);
    if (procThread) 
    {
        ResumeThread(procThread);
    }
}
