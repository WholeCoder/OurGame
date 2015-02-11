namespace OurGame.OurGameLibrary.TemplateMethod
{
    class RetrieveTilesLessThanCurrentSpriteYPositionTemplateMethod : RetrieveTilesTemplateMethod
    {
        public override string ToString()
        {
            return "Gets tiles that have y co-oridnate less than the sprite's y coordinate. - " + base.ToString();
        }

        public override bool ShouldKeepOnlyTilesBelowCurrentPositionY()
        {
            return true;
        }
    }
}
