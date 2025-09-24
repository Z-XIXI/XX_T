using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class GameTimeManager : MonoBehaviour
{

    public static GameTimeManager Instance;

    private float _totalGameTiem;
    public float TotalGameTime { get => _totalGameTiem; }

    void Awake()
    {
        // 简单的单例初始化
        if (Instance == null)
        {
            Instance = this;
            DelayClass.Instance = new DelayClass();
            DelayClass.Instance._monoBehaviour = this;
            RuntimeClass.Instance = new RuntimeClass();
            RuntimeClass.Instance._monoBehaviour = this;
        }
    }

    private void Update()
    {
        this._totalGameTiem += Time.deltaTime;
    }


    /// <summary>
    /// 带任务ID的延迟调用
    /// </summary>
    public static void AddDelay(string taskId, System.Action action, float delaySeconds)
    {
        DelayClass.Instance.AddDelayTask(taskId, action, delaySeconds);
    }

    /// <summary>
    /// 取消指定任务
    /// </summary>
    public static void CancelDekat(string taskId)
    {
        DelayClass.Instance.RemoveDelayTask(taskId);
    }

    /// <summary>
    /// 取消所有延迟任务
    /// </summary>
    public static void CancelAllDekat()
    {
        DelayClass.Instance.RemoveAllDelayTasks();
    }

    /// <summary>
    /// 添加倒计时
    /// </summary>
    public static void AddRuntime(float totalDuration, float interval, Action<int> onInterval = null, Action onComplete = null)
    {

    }


    /// <summary>
    /// 延时类
    /// </summary>
    private class DelayClass
    {
        public static DelayClass Instance;
        public MonoBehaviour _monoBehaviour;

        private Dictionary<string, Coroutine> _delayTasks = new Dictionary<string, Coroutine>();
        /// <summary>
        /// 添加延迟任务（带任务ID）
        /// </summary>
        public void AddDelayTask(string taskId, System.Action action, float delaySeconds)
        {
            // 如果任务已存在，先取消
            if (_delayTasks.ContainsKey(taskId))
            {
                RemoveDelayTask(taskId);
            }

            Coroutine coroutine = _monoBehaviour.StartCoroutine(DelayCoroutine(taskId, action, delaySeconds));
            _delayTasks[taskId] = coroutine;
        }
        /// <summary>
        /// 移除延迟任务
        /// </summary>
        public void RemoveDelayTask(string taskId)
        {
            if (_delayTasks.TryGetValue(taskId, out Coroutine coroutine))
            {
                if (coroutine != null)
                {
                    _monoBehaviour.StopCoroutine(coroutine);
                }
                _delayTasks.Remove(taskId);
            }
        }
        /// <summary>
        /// 移除所有延迟任务
        /// </summary>
        public void RemoveAllDelayTasks()
        {
            foreach (var coroutine in _delayTasks.Values)
            {
                if (coroutine != null)
                {
                    _monoBehaviour.StopCoroutine(coroutine);
                }
            }
            _delayTasks.Clear();
        }
        /// <summary>
        /// 检查任务是否存在
        /// </summary>
        public bool HasTask(string taskId)
        {
            return _delayTasks.ContainsKey(taskId);
        }
        private IEnumerator DelayCoroutine(string taskId, System.Action action, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            action?.Invoke();

            // 执行完成后从字典中移除
            if (_delayTasks.ContainsKey(taskId))
            {
                _delayTasks.Remove(taskId);
            }
        }
    }

    private class RuntimeClass : MonoBehaviour
    {
        public static DelayClass Instance;
        public MonoBehaviour _monoBehaviour;
        private class TimerConfig
        {
            public float _totalDuration;
            public float _interval;
            public Action<int> _onInterval;
            public Action _onComplete;
            public TimerConfig(float totalDuration, float interval, Action<int> onInterval = null, Action onComplete = null)
            {
                this._totalDuration = totalDuration;
                this._interval = interval;
                this._onInterval = onInterval;
                this._onComplete = onComplete;
            }
        }

        private Dictionary<string, Coroutine> _runtimeTasks = new Dictionary<string, Coroutine>();
        public void AddRuntimeTask(string timerId, float totalDuration, float interval, Action<int> onInterval = null, Action onComplete = null)
        {
            if (_runtimeTasks.ContainsKey(timerId))
            {
                StopTimer(timerId);
            }

            TimerConfig config = new TimerConfig(totalDuration, interval, onInterval, onComplete);
            _runtimeTasks[timerId] = _monoBehaviour.StartCoroutine(TimerRoutine(timerId, config));
        }

        public void StopTimer(string timerId)
        {
            if (_runtimeTasks.TryGetValue(timerId, out Coroutine coroutine))
            {
                _monoBehaviour.StopCoroutine(coroutine);
                _runtimeTasks.Remove(timerId);
            }
        }

        private IEnumerator TimerRoutine(string timerId, TimerConfig config)
        {
            float nextIntervalTime = config._interval;
            handle.elapsedTime = 0f;
            handle.triggerCount = 0;

            while (handle.elapsedTime < config.totalDuration && handle.isActive)
            {
                if (config.ignoreTimeScale)
                {
                    yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
                    handle.elapsedTime += Time.unscaledDeltaTime;
                }
                else
                {
                    yield return null;
                    handle.elapsedTime += Time.deltaTime;
                }

                // 检查间隔触发
                if (handle.elapsedTime >= nextIntervalTime && handle.isActive)
                {
                    handle.triggerCount++;
                    config.onInterval?.Invoke(handle.triggerCount);
                    nextIntervalTime += config.interval;
                }
            }

            // 计时器完成
            if (handle.isActive)
            {
                handle.isActive = false;
                config.onComplete?.Invoke();
            }

            _activeTimers.Remove(timerId);
        }
    }
}