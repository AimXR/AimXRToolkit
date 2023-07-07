using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using AimXRToolkit;


namespace AimXRToolkit.Managers
{
    [AddComponentMenu("AimXR/Scripts/Activity Manager")]
    public class ActivityManager : MonoBehaviour
    {
        private Models.Activity _activity;
        private Models.Action _currentAction;

        public Performance.Performance performance;

        [Header("Event launched after an action changed")]
        [SerializeField]
        public UnityEngine.Events.UnityEvent<Models.Action> onActionChange;

        [Header("Event launched when the action start")]
        [SerializeField]
        public UnityEvent<Models.Action> onActionStart;

        [SerializeField]
        [Header("Event launched when the action end")]
        public UnityEvent<Models.Action> onActionEnd;

        [Header("Event launched when the action is played less than one second")]
        [SerializeField]
        public UnityEngine.Events.UnityEvent<Models.Action> onActionSkip;

        [Header("Event launched when the activity start")]
        [SerializeField]
        public UnityEngine.Events.UnityEvent<Models.Activity> onActivityStart;

        [Header("Event launched when the activity end")]
        [SerializeField]
        public UnityEngine.Events.UnityEvent<Models.Activity> onActivityEnd;

        private bool _started;
        private float lastNext;

        void Start()
        {
        }
        public void SetActivity(Models.Activity activity)
        {
            this._activity = activity;
            _currentAction = null;
        }

        public Models.Activity GetActivity()
        {
            return _activity;
        }

        public async Task<bool> PreviousAction()
        {

            if (_currentAction != null)
            {
                if (_currentAction.GetPrevious() < 1)
                {
                    return false;
                }
                onActionEnd.Invoke(_currentAction);
                performance.ActionComplete(_currentAction);
                _currentAction = await Managers.DataManager.GetInstance().GetActionAsync(_currentAction.GetPrevious());
                onActionStart.Invoke(_currentAction);
                onActionChange.Invoke(_currentAction);
                performance.ActionStart(_currentAction);
            }
            return _currentAction != null;
        }
        public async Task<bool> NextAction()
        {
            // if current action is null, the we are at the beginning of the activity and the first action is contain in the activity object
            if (_currentAction == null)
            {
                if (!_started)
                {
                    onActivityStart.Invoke(_activity);
                    _started = true;
                }
                _currentAction = await Managers.DataManager.GetInstance().GetActionAsync(_activity.GetStart());
                onActionChange.Invoke(_currentAction);
                onActionStart.Invoke(_currentAction);
                lastNext = Time.time;
                await performance.ActivityStart(_activity);
                performance.ActionStart(_currentAction);
            }
            else
            {
                float time = Time.time;
                if (time - lastNext < 1.0f)
                {
                    onActionSkip.Invoke(_currentAction);
                    try
                    {
                        performance.ActionSkip(_currentAction);

                    }
                    catch (System.Exception)
                    {
                    }
                }
                else
                {
                    onActionEnd.Invoke(_currentAction);
                    try
                    {
                        performance.ActionComplete(_currentAction);

                    }
                    catch (System.Exception)
                    {
                    }
                }
                if (_currentAction.GetNext() < 1)
                {
                    onActivityEnd.Invoke(_activity);
                    performance.ActivityEnd(_activity);
                    return false;
                }
                _currentAction = await Managers.DataManager.GetInstance().GetActionAsync(_currentAction.GetNext());
                onActionStart.Invoke(_currentAction);
                onActionChange.Invoke(_currentAction);
                try
                {
                    performance.ActionStart(_currentAction);
                }
                catch (System.Exception)
                {
                }
                lastNext = Time.time;
            }

            return _currentAction != null;
        }
        public Models.Action GetCurrentAction()
        {
            return _currentAction;
        }

        public void HelpRequested()
        {
            performance.ActionHelp(_currentAction);
        }
        public void ReplayRequested()
        {

        }
        public void ReloadActivity()
        {
            _currentAction = null;
            _started = false;
            performance.Reset();
        }
    }
}

