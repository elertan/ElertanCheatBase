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
        private const string OpenGlLibrary = "opengl32.dll";
        private const string WglSwapBuffersFuncName = "wglSwapBuffers";
        private HookBase _hookBase;
        private LocalHook _wglSwapBuffersHook;

        public void Install(HookBase hookBase)
        {
            _hookBase = hookBase;

            return;

            var moduleHandle = WinApi.GetModuleHandle(OpenGlLibrary);

            var wglSwapBuffersFuncIntPtr = WinApi.GetProcAddress(moduleHandle, WglSwapBuffersFuncName);

            _wglSwapBuffersHook = LocalHook.Create(wglSwapBuffersFuncIntPtr, new DSwapBuffers(SwapBuffersHook), this);
            _wglSwapBuffersHook.ThreadACL.SetExclusiveACL(new[] { 0 });   
        }

        [DllImport(OpenGlLibrary, EntryPoint = WglSwapBuffersFuncName, ExactSpelling = true, SetLastError = true)]
        private static extern bool SwapBuffers(IntPtr hdc);

        private bool SwapBuffersHook(IntPtr hdc)
        {
            //var context = Wgl.CreateContext(hdc);
            Prepare2D();
            //// Do drawing

            Gl.Color3(255, 0, 0);
            Gl.Begin(PrimitiveType.Quads);

            Gl.Vertex2(150, 150);
            Gl.Vertex2(500, 150);
            Gl.Vertex2(500, 500);
            Gl.Vertex2(150, 500);

            Gl.End();

            RestoreOpenGl();
            return SwapBuffers(hdc);
        }

        private void Prepare2D()
        {
            //Gl.PushAttrib(AttribMask.AllAttribBits);
            Gl.MatrixMode(MatrixMode.Projection);
            Gl.LoadIdentity();
            // WindowWidth, WindowHeight
            Gl.Ortho(0, 1600, 1200, 0, -1.0f, -1.0f);
            Gl.Disable(EnableCap.Texture2d);
            Gl.Disable(EnableCap.Blend);
            Gl.Disable(EnableCap.DepthTest);
        }

        private void RestoreOpenGl()
        {
            //Gl.PopAttrib();
            Gl.MatrixMode(MatrixMode.Projection);
            Gl.LoadIdentity();
        }

        public void Uninstall()
        {
            _wglSwapBuffersHook?.Dispose();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate bool DSwapBuffers(IntPtr hdc);
    }
}
