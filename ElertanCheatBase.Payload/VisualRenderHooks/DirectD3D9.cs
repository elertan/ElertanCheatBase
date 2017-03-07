using System;
using System.Runtime.InteropServices;
using EasyHook;
using ElertanCheatBase.Payload.Interfaces;
using SharpDX;
using SharpDX.Direct3D9;

namespace ElertanCheatBase.Payload.VisualRenderHooks
{
    internal class DirectD3D9 : IHook
    {
        private LocalHook _drawIndexedPrimitiveHook;
        private LocalHook _endSceneLocalHook;
        private HookBase _hookBase;

        public void Install(HookBase hookBase)
        {
            _hookBase = hookBase;

            using (var direct3D = new Direct3D())
            {
                var device = new Device(
                    direct3D,
                    0,
                    DeviceType.NullReference,
                    IntPtr.Zero,
                    CreateFlags.HardwareVertexProcessing,
                    new PresentParameters {BackBufferWidth = 1, BackBufferHeight = 1});

                var baseAddress = Marshal.ReadIntPtr(device.NativePointer);

                // EndSceneHook
                _endSceneLocalHook =
                    LocalHook.Create(Marshal.ReadIntPtr(baseAddress, (int) VmtMethods.EndScene * IntPtr.Size),
                        new DEndScene(EndSceneHook), this);
                _endSceneLocalHook.ThreadACL.SetExclusiveACL(new[] {0});

                // DrawIndexedPrimitiveHook
                _drawIndexedPrimitiveHook =
                    LocalHook.Create(
                        Marshal.ReadIntPtr(baseAddress, (int) VmtMethods.DrawIndexedPrimitive * IntPtr.Size),
                        new DDrawIndexedPrimitive(DrawIndexedPrimitiveHook), this);
                _drawIndexedPrimitiveHook.ThreadACL.SetExclusiveACL(new[] {0});
            }
        }

        public void Uninstall()
        {
            _endSceneLocalHook?.Dispose();
        }

        public int EndSceneHook(IntPtr devicePtr)
        {
            var device = (Device) devicePtr;
            // Handle Endscene
            _hookBase.Direct3D9_EndScene(device);
            device.EndScene();
            return Result.Ok.Code;
        }

        public int DrawIndexedPrimitiveHook(IntPtr devicePtr,
            PrimitiveType primitiveType,
            int baseVertexIndex,
            int minVertexIndex,
            int numVertices,
            int startIndex,
            int primCount)
        {
            var device = (Device) devicePtr;

            device.DrawIndexedPrimitive(primitiveType, baseVertexIndex, minVertexIndex, numVertices, startIndex,
                primCount);

            return Result.Ok.Code;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate int DEndScene(IntPtr devicePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        private delegate int DDrawIndexedPrimitive(IntPtr devicePtr,
            PrimitiveType primitiveType,
            int baseVertexIndex,
            int minVertexIndex,
            int numVertices,
            int startIndex,
            int primCount);

        internal enum VmtMethods
        {
            QueryInterface,
            AddRef,
            Release,
            TestCooperativeLevel,
            GetAvailableTextureMem,
            EvictManagedResources,
            GetDirect3D,
            GetDeviceCaps,
            GetDisplayMode,
            GetCreationParameters,
            SetCursorProperties,
            SetCursorPosition,
            ShowCursor,
            CreateAdditionalSwapChain,
            GetSwapChain,
            GetNumberOfSwapChains,
            Reset,
            Present,
            GetBackBuffer,
            GetRasterStatus,
            SetDialogBoxMode,
            SetGammaRamp,
            GetGammaRamp,
            CreateTexture,
            CreateVolumeTexture,
            CreateCubeTexture,
            CreateVertexBuffer,
            CreateIndexBuffer,
            CreateRenderTarget,
            CreateDepthStencilSurface,
            UpdateSurface,
            UpdateTexture,
            GetRenderTargetData,
            GetFrontBufferData,
            StretchRect,
            ColorFill,
            CreateOffscreenPlainSurface,
            SetRenderTarget,
            GetRenderTarget,
            SetDepthStencilSurface,
            GetDepthStencilSurface,
            BeginScene,
            EndScene,
            Clear,
            SetTransform,
            GetTransform,
            MultiplyTransform,
            SetViewport,
            GetViewport,
            SetMaterial,
            GetMaterial,
            SetLight,
            GetLight,
            LightEnable,
            GetLightEnable,
            SetClipPlane,
            GetClipPlane,
            SetRenderState,
            GetRenderState,
            CreateStateBlock,
            BeginStateBlock,
            EndStateBlock,
            SetClipStatus,
            GetClipStatus,
            GetTexture,
            SetTexture,
            GetTextureStageState,
            SetTextureStageState,
            GetSamplerState,
            SetSamplerState,
            ValidateDevice,
            SetPaletteEntries,
            GetPaletteEntries,
            SetCurrentTexturePalette,
            GetCurrentTexturePalette,
            SetScissorRect,
            GetScissorRect,
            SetSoftwareVertexProcessing,
            GetSoftwareVertexProcessing,
            SetNPatchMode,
            GetNPatchMode,
            DrawPrimitive,
            DrawIndexedPrimitive,
            DrawPrimitiveUP,
            DrawIndexedPrimitiveUP,
            ProcessVertices,
            CreateVertexDeclaration,
            SetVertexDeclaration,
            GetVertexDeclaration,
            SetFVF,
            GetFVF,
            CreateVertexShader,
            SetVertexShader,
            GetVertexShader,
            SetVertexShaderConstantF,
            GetVertexShaderConstantF,
            SetVertexShaderConstantI,
            GetVertexShaderConstantI,
            SetVertexShaderConstantB,
            GetVertexShaderConstantB,
            SetStreamSource,
            GetStreamSource,
            SetStreamSourceFreq,
            GetStreamSourceFreq,
            SetIndices,
            GetIndices,
            CreatePixelShader,
            SetPixelShader,
            GetPixelShader,
            SetPixelShaderConstantF,
            GetPixelShaderConstantF,
            SetPixelShaderConstantI,
            GetPixelShaderConstantI,
            SetPixelShaderConstantB,
            GetPixelShaderConstantB,
            DrawRectPatch,
            DrawTriPatch,
            DeletePatch,

            CreateQuery
        }
    }
}