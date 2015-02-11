using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.OurGameLibrary.TemplateMethod
{
    class RetrieveTilesLessThanCurrentSpriteYPositionTemplateMethod : RetrieveTilesTemplateMethod
    {
        public override bool ShouldKeepOnlyTilesBelowCurrentPositionY()
        {
            return true;
        }
    }
}
