using Assets.Scripts.AI.FCS;

namespace Assets.Scripts.AI
{
	public struct RocketContact
	{
		public readonly RocketComponent Rocket;
		public readonly RocketClassification Classification;

		public RocketContact(RocketComponent rocket, RocketClassification classification)
		{
			Rocket = rocket;
			Classification = classification;
		}
	}
}