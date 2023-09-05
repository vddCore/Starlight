using System.Drawing;
using Starlight.Asus;
using Starlight.Asus.AnimeMatrix;
using Starlight.Asus.AuraKeyboard;

// using var anime = new AnimeMatrixDevice();
//
// while (true)
// {
//     Console.Clear();
//     
//     var config = anime.GetConfig();
//
//     for (var i = 0; i < config.Length; i++)
//     {
//         Console.Write($"{config[i]:X2} ");
//     }
// }

using var aura = new AuraKeyboardDevice();
var status = aura.QueryStatus();
var layout = aura.QueryKeyboardLayout();
var defaultMode = aura.QueryDefaultModes();

aura.Mode.Static(Color.Aqua);

// Task.Run(() =>
// {
//     while (true)
//     {
//         for (var i = 0; i < 256; i++)
//         {
//             aura.Mode.Static(Color.FromArgb(0, i, 255 - i));
//         }
//
//         for (var i = 255; i >= 0; i--)
//         {
//             aura.Mode.Static(Color.FromArgb(0, i, 255 - i));
//         }
//     }
// });
//
// Task.Run(async () =>
// {
//     while (true)
//     {
//         aura.SetBrightness(BrightnessLevel.Full);
//         await Task.Delay(0);
//         aura.SetBrightness(BrightnessLevel.Off);
//         await Task.Delay(250);
//         aura.SetBrightness(BrightnessLevel.Medium);
//         await Task.Delay(250);
//     }
// });
//
// while (true)
// {
//     await Task.Delay(1);
// }


// aura.Mode.Pulse(Color.White, AuraAnimationSpeed.Slow);
// aura.Mode.Pulse(Color.White, AuraAnimationSpeed.Slow);
