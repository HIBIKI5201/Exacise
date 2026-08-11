namespace NavyGame.Runtime
{
    /// <summary>
    ///     タレットの初期化を行うクラス。
    /// </summary>
    public static class TurretInitializer
    {
        public static void Init(TurretViewContainer container, TargetContainer targetContainer, out Result result)
        {
            TurretStatus status = container.Status;

            TargetSystem targetSystem = new(container, targetContainer, status.Range);

            TurretRotater rotater = new(container);
            TurretShooter shooter = new(container, status);
            TurretLifeCycle lifeCycle = new(rotater, shooter, targetSystem);
            result = new Result(lifeCycle);
        }

        public readonly ref struct Result
        {
            public Result(TurretLifeCycle lifeCycle)
            {
                LifeCycle = lifeCycle;
            }

            public readonly TurretLifeCycle LifeCycle;
        }
    }
}
