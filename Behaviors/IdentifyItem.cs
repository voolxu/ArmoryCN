using System;
using System.Linq;
using System.Threading.Tasks;
using Armory.Settings;
using Buddy.Coroutines;
using Zeta.Bot;
using Zeta.Game;
using Zeta.Game.Internals.Actors;
using Zeta.TreeSharp;

namespace Armory.Behaviors
{
    public class IdentifyBehavior
    {
        private const int IdentifyDelay = 7000;

        private static float _startHealth;
        private static DateTime _lastIdentifyRequest = DateTime.MinValue;

        private static ACDItem UnIdentifiedItem
        {
            get
            {
                if (!ZetaDia.IsInGame || ZetaDia.IsLoadingWorld || ZetaDia.Me == null)
                    return null;
                if (!ZetaDia.Me.IsValid)
                    return null;

                return ZetaDia.Me.Inventory.Backpack.FirstOrDefault(i => i.IsValid && i.IsUnidentified);
            }
        }

        public static Composite CreateBehavior()
        {
            return new ActionRunCoroutine(ret => IdentifyTask());
        }

        public static async Task<bool> IdentifyTask()
        {

            if (!ArmorySettings.Instance.IdentifyItems)
                return false;

            if (!ZetaDia.IsInGame || ZetaDia.IsLoadingWorld)
                return false;

            if (ZetaDia.Me == null || (ZetaDia.Me != null && !ZetaDia.Me.IsValid))
                return false;

            if (!Player.IsValid)
                return false;

            while (ZetaDia.Me.LoopingAnimationEndTime > 0 && ZetaDia.Me.HitpointsCurrent >= _startHealth)
            {
                await Coroutine.Yield();
            }

            if (ZetaDia.Me.HitpointsCurrent < _startHealth)
            {
                _startHealth = ZetaDia.Me.HitpointsCurrent;
                return false;
            }

            if (IdentifyDelayReady() && UnIdentifiedItem != null)
            {
                _startHealth = ZetaDia.Me.HitpointsCurrent;
                await Coroutine.Sleep(500);
                Logger.Log("Identifying item {0}", UnIdentifiedItem.Name);
                ZetaDia.Me.Inventory.IdentifyItem(UnIdentifiedItem.DynamicId);
                _lastIdentifyRequest = DateTime.UtcNow;
                await Coroutine.Sleep(500);

                while ((DateTime.UtcNow.Subtract(_lastIdentifyRequest).TotalMilliseconds < 500 || ZetaDia.Me.LoopingAnimationEndTime > 0) && ZetaDia.Me.HitpointsCurrent >= _startHealth)
                {
                    await Coroutine.Yield();
                }
                return true;
            }
            return false;
        }

        private static bool IdentifyDelayReady()
        {
            return DateTime.UtcNow.Subtract(_lastIdentifyRequest).TotalMilliseconds > IdentifyDelay;
        }
    }
}
