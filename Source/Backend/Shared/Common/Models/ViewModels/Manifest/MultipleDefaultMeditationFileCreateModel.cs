#pragma warning disable CS0108,SA1502,S4015
using System.Collections.Generic;

namespace Manifest.Common.Models.ViewModels
{
    public class MultipleDefaultMeditationFileCreateModel
    {
        public List<DefaultMeditationFileCreateModel> NewDefaultMeditationFiles { get; set; }
    }
}
#pragma warning restore CS0108, SA1502, S4015