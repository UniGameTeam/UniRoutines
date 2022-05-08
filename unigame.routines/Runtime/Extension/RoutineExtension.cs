﻿namespace UniModules.UniRoutine.Runtime.Extension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniGame.Core.Runtime.Interfaces;
    using UniRoutines.unigame.routines.Assets.UniGame.Routines.Runtime.Extension;
    using UnityEngine;
    using UnityEngine.Profiling;
    using Object = UnityEngine.Object;
    using WaitForEndOfFrame = UniRoutines.unigame.routines.Assets.UniGame.Routines.Runtime.Extension.WaitForEndOfFrame;

    public static partial class RoutineExtension
    {

        public static IEnumerator RoutineWaitUntil(this ICompletionStatus status) {

            if(status == null)yield break;
            
            while (status.IsComplete == false) {
                yield return null;
            }

        }

        public static IEnumerator RoutineWaitUntil(this Func<bool> completeFunc)
        {

            if (completeFunc == null) yield break;
            while (completeFunc() == false)
            {
                yield return null;
            }

        }

        public static IEnumerator OnUpdate(this object target,Func<float,bool> updateAction)
        {
            
            if (updateAction == null) yield break;

            var isMoveNext = false;
            var deltaTime = Time.realtimeSinceStartup;
            
            do {
                
                var delta = Time.realtimeSinceStartup - deltaTime;
                deltaTime = Time.realtimeSinceStartup;
                isMoveNext = updateAction(delta);
                
                yield return null;
                
            } while (isMoveNext);

        }

        public static IEnumerator DoDelayed(this object target,Action action)
        {
            yield return null;
            action?.Invoke();
        }

        public static IEnumerator DoDelayed(this Action action)
        {
            yield return null;
            action?.Invoke();
        }
        
        public static IEnumerator WaitForSecond(this object source,float delay)
        {
            var time = Time.time + delay;
            while (time < Time.time)
            {
                yield return null;
            }
        }
        
        public static IEnumerator WaitForSecondUnscaled(this object source,float delay)
        {
            var time = Time.unscaledTime;
            var endOfAwait = time + delay;
            while (time < endOfAwait)
            {
                yield return null;
                time = Time.unscaledTime;
            }
        }

        public static IEnumerator WaitForSeconds(this object source,float delay)
        {
            var waitForSeconds = ClassPool.
                Spawn<WaitForSecondRoutine>().
                Initialize(delay);
            
            yield return waitForSeconds;
            
            waitForSeconds.DespawnWithRelease();
        }

        public static IEnumerator WaitForEndOfFrame(this object source)
        {
            var waitForEndFrame = ClassPool.Spawn<WaitForEndOfFrame>().Initialize();

            yield return waitForEndFrame;
            
            waitForEndFrame.DespawnWithRelease();
        }

        public static IEnumerator WaitUntil(this object source, Func<bool> completeFunc)
        {

            if (source == null || completeFunc == null) yield break;
            while (completeFunc() == false)
            {
                yield return null;
            }

        }

        public static IEnumerator WaitWhile(this object source, Func<bool> completeFunc)
        {

            if (completeFunc == null) yield break;
            while (completeFunc())
            {
                yield return null;
            }

        }

        public static IEnumerator DoWhile(this Object target, Func<IEnumerator> sequence, Func<bool> condition)
        {
            
            if(sequence == null || condition == null)yield break;

            while (target && condition())
            {
                yield return sequence();
                yield return null;
            }
            
        }

        public static IDisposable ExecuteWithCondition(this object target, 
            Action action, Func<bool> condition,
            Func<bool> awaiter,
            RoutineType routineType = RoutineType.Update)
        {
            
            var enumerator = ExecuteWhen(target,action, condition,awaiter);
            var disposable = enumerator.RunWithSubRoutines(routineType);
            return disposable;
            
        }

        public static IDisposable ExecuteWithCondition(this object target, 
            Action action, Func<bool> condition,
            RoutineType routineType = RoutineType.Update)
        {
            
            var enumerator = ExecuteWhile(target,action, condition);
            var disposable = enumerator.RunWithSubRoutines(routineType);
            return disposable;
            
        }
        
        /// <summary>
        /// execute target action when condition is true, repeat until awaiter is true
        /// </summary>
        /// <param name="target">target object</param>
        /// <param name="action">target action</param>
        /// <param name="condition">action condition</param>
        /// <param name="awaiter">awaiter</param>
        /// <returns>progress awaiter</returns>
        public static IEnumerator ExecuteWhen(this object target, Action action, Func<bool> condition,
            Func<bool> awaiter)
        {
                        
            while (awaiter())
            {
                if (condition())
                {
                    action();
                }
                yield return null;
            }

        }
        
        
        /// <summary>
        /// repeat target action until condition is true
        /// </summary>
        /// <returns>progress enumerator</returns>
        public static IEnumerator ExecuteWhile(this object target, Action action, Func<bool> condition)
        {

            while (condition())
            {
                action();
                yield return null;
            }
            
        }
        
        public static IEnumerator RoutineWaitUntil(this AsyncOperation operation) {

            if(operation == null)yield break;
            
            while (operation.isDone == false) {
                yield return null;
            }

        }
        
    }
}
