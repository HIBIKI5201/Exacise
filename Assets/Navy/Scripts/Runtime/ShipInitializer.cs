namespace NavyGame.Runtime
{
    public static class ShipInitializer
    {
        public static void Init(ShipViewContainer container, TargetContainer targetContainer, InputBuffer input, out Result result)
        {
            ShipStatus status = container.Status;

            ShipMover mover = new(status, container);
            ShipLifeCycle lifeCycle = new(input, mover);

            TurretLifeCycle[] turretResults = new TurretLifeCycle[container.Turrets.Length];
            for (int i = 0; i < container.Turrets.Length; i++)
            {
                TurretInitializer.Init(
                    container.Turrets[i], 
                    targetContainer, 
                    out TurretInitializer.Result rlt);
                turretResults[i] = rlt.LifeCycle;
            }

            result = new Result(lifeCycle, turretResults);
        }

        public readonly ref struct Result
        {
            public Result(ShipLifeCycle ship, TurretLifeCycle[] turrets)
            {
                Ship = ship;
                Turrets = turrets;
            }

            public readonly ShipLifeCycle Ship;
            public readonly TurretLifeCycle[] Turrets;
        }
    }
}
