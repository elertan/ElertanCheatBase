using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyHook;
using ElertanCheatBase.Payload.Interfaces;
using OpenGL;

namespace ElertanCheatBase.Payload.VisualRenderHooks
{
    internal class OpenGl : IHook
    {
        private HookBase _hookBase;
        private LocalHook _glBeginLocalHook;
        private LocalHook _glEndLocalHook;
        private DeviceContext _device;

        public void Install(HookBase hookBase)
        {
            _hookBase = hookBase;

            var moduleHandle = WinApi.GetModuleHandle("opengl32.dll");

            var glBeginFuncIntPtr = WinApi.GetProcAddress(moduleHandle, "glBegin");
            var glEndFuncIntPtr = WinApi.GetProcAddress(moduleHandle, "glEnd");

            // Need to hook into SwapBuffers
            // Look at WglCreateContext
            // Need to retrieve a handle to the current device
            _device = DeviceContext.Create();
            var ctx = _device.CreateContext(IntPtr.Zero);
            _device.MakeCurrent(ctx);

            _glBeginLocalHook = LocalHook.Create(glBeginFuncIntPtr, new DGlBegin(GlBeginHook), this);
            _glBeginLocalHook.ThreadACL.SetExclusiveACL(new[] { 0 });

            _glEndLocalHook = LocalHook.Create(glEndFuncIntPtr, new DGlEnd(GlEndHook), this);
            _glEndLocalHook.ThreadACL.SetExclusiveACL(new[] { 0 });
        }

        public void Uninstall()
        {
            _glBeginLocalHook?.Dispose();
            _glEndLocalHook?.Dispose();
        }

        private void GlBeginHook(PrimitiveType mode)
        {
            _hookBase.OpenGL_GlBegin(_device, mode);

            Gl.Begin(mode);
        }

        private void GlEndHook()
        {
            Gl.End();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate void DGlBegin(PrimitiveType mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate void DGlEnd();
    }
}
