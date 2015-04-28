using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Dynamics_Final_Project
{
    public static class ExtentionsCamera
    {
        public static void BeginCamera(this SpriteBatch mBatch, Camera2D camera)
        {
            mBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, camera.GetViewTransformationMatrix());
        }

        public static void BeginCamera(this SpriteBatch mBatch, Camera2D camera, BlendState bstate)
        {
            mBatch.Begin(SpriteSortMode.Deferred, bstate, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, camera.GetViewTransformationMatrix());
        }

    }
}
