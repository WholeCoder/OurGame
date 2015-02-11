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
