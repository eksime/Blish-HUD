﻿using System;
using System.Runtime.InteropServices;
using Blish_HUD.DebugHelper.Native;
using Blish_HUD.DebugHelperLib.Models;
using Blish_HUD.DebugHelperLib.Services;

namespace Blish_HUD.DebugHelper.Services {

    internal class KeyboardHookService : IDebugService, IDisposable {

        private const int CALLBACK_TIMEOUT = 10;

        private readonly IMessageService messageService;
        private readonly User32.HOOKPROC hookProc; // Store the callback delegate, otherwise it might get garbage collected
        private IntPtr hook;

        public KeyboardHookService(IMessageService messageService) {
            this.messageService = messageService;
            hookProc = HookCallback;
        }

        public void Start() {
            if (hook == IntPtr.Zero) {
                hook = User32.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, hookProc, Marshal.GetHINSTANCE(typeof(MouseHookService).Module), 0);
            }
        }

        public void Stop() {
            if (hook != IntPtr.Zero) {
                User32.UnhookWindowsHookEx(hook);
            }
            hook = IntPtr.Zero;
        }

        private int HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode != 0)
                return User32.CallNextHookEx(HookType.WH_KEYBOARD_LL, nCode, wParam, lParam);

            var eventType = (uint)wParam % 2 + 256; // filter out SysKeyDown & SysKeyUp
            var key = Marshal.ReadInt32(lParam);

            var response = messageService.SendAndWait<KeyboardResponseMessage>(new KeyboardEventMessage {
                EventType = eventType,
                Key = key
            }, TimeSpan.FromMilliseconds(CALLBACK_TIMEOUT));

            if (response?.IsHandled == true)
                return 1;
            else
                return User32.CallNextHookEx(HookType.WH_MOUSE_LL, nCode, wParam, lParam);
        }

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool isDisposing) {
            if (!isDisposed) {
                if (isDisposing) {
                    Stop();
                }
                isDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
