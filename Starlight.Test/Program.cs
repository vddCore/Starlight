using Starlight.AnimeMatrix;
using Starlight.Engine;

using var mat = new AnimeMatrixDevice();
using var renderer = new AnimeMatrixRenderer(mat);

renderer.LoadScript("anim.lua");
await renderer.Run(60);