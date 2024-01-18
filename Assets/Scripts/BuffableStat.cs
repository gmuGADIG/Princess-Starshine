// Use this for stats that other code may change
// Basic usage:



using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/*
public class MyComponent : MonoBehaviour {
    public float initialFunny = 3f;
    [HideInInspector] public BuffableStat funny;

    // Start is called before the first frame update
    void Start() {
        funny = new BuffableStat(initialFunny);

        Debug.Log(funny.Value); // => 3.0
        
        BuffableStat.Receipt receipt = funny.AddBuff(66);
        Debug.Log(funny.Value); // => 69.0
        receipt.Unbuff();
        Debug.Log(funny.Value); // => 3.0

        BuffableStat.Receipt receipt1 = funny.MultiplierBuff(3);
        Debug.Log(funny.Value); // => 9.0
        BuffableStat.Receipt receipt2 = funny.AddBuff(2);
        Debug.Log(funny.Value); // => 15.0
        receipt1.Unbuff();
        Debug.Log(funny.Value); // => 5.0
        receipt2.Unbuff();

        // You can even do debuffs!
        BuffableStat.Receipt debuff1 = funny.AddBuff(-2);
        Debug.Log(funny.Value); // => 1.0
        BuffableStat.Receipt debuff2 = funny.MultiplierBuff(1/4);
        Debug.Log(funny.Value); // => 0.25
        debuff1.Unbuff();
        debuff2.Unbuff();
    }
}
*/

/// <summary>
/// Stat that can be buffed and unbuffed easily.
/// </summary>
public class BuffableStat {
    public UnityEvent<float> ValueUpdated = new UnityEvent<float>();

    public enum Order { 
        AddThenMultiply, // bigger numbers, default
        MultiplyThenAdd // smaller numbers
    }
    private Order order;
    private float multiplier = 1;
    private float adds = 0;
    private float initial;
    
    /// <summary>
    /// The value of the stat after all of the buffs have been applied.
    /// </summary>
    public float Value { get {
        if (order == Order.AddThenMultiply)
            return (initial + adds) * multiplier;
        else
            return (initial * multiplier) + adds;
    } }

    /// <summary>
    /// A log of the buff you applied to a <c>BuffableStat</c>. Can be unapplied with <c>BuffableStat.Receipt.Unbuff()</c>.
    /// </summary>
    public class Receipt {
        private static bool quitting = false;
        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart() {
            Application.quitting += () => { quitting = true; };
        }

        public enum Type { Add, Multiply }
        /// <summary>
        /// The amount this receipt is modifying the underlying BuffableStat
        /// </summary>
        public float Value { get; private set; }
        private Type type;
        private string callerFilePath;
        private int callerLineNumber;

        public BuffableStat Stat { get; private set; }

        private bool unapplied = false;

        public Receipt(
                BuffableStat bs, 
                Type type, 
                float value,
                string callerFilePath,
                int callerLineNumber
        ) {
            this.Stat = bs;
            this.Value = value;
            this.type = type;
            this.callerFilePath = callerFilePath;
            this.callerLineNumber = callerLineNumber;
        }

        /// <summary>
        /// Swap out this receipt for a new receipt that has new buff value.
        /// Equivalent to <c>Receipt.Unbuff(); BuffableStat.{AddBuff,MultiplierBuff}(amount)</c>.
        /// Will reuse Multiply/Add type.
        /// </summary>
        /// <param name="amount">Value of new buff</param>
        /// <returns>Receipt representing the new buff.</returns>
        public Receipt Rebuff(float amount) {
            Unbuff();
            if (type == Type.Add)
                return Stat.AddBuff(amount);
            else 
                return Stat.MultiplierBuff(amount);
        }

        /// <summary>
        /// Unapply the buff this receipt represents. This is an idempotent operation.
        /// </summary>
        public void Unbuff() {
            if (!unapplied) {
                if (type == Type.Add)
                    Stat.adds -= Value;
                else if (type == Type.Multiply)
                    Stat.multiplier /= Value;

                unapplied = true;
                Stat.ValueUpdated.Invoke(Stat.Value);
            } 
        }

        ~Receipt() { // destructor :P
            if (!unapplied && !quitting) {
                string errorMessage = "Someone forgot to unapply a buff!";

                errorMessage += "\ncallerLineNumber: " + callerLineNumber;
                errorMessage += " | callerFilePath: " + callerFilePath;

                Debug.LogWarning(errorMessage);
            }
        }
    }

    public BuffableStat(float initialValue, Order order = Order.AddThenMultiply) {
        this.initial = initialValue;
        this.order = order;
    }

    /// <summary>
    /// Add <paramref name="amount"/> to this <c>BuffableStat</c>'s value.
    /// </summary>
    public Receipt AddBuff(
            float amount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
    ) {
        adds += amount;
        ValueUpdated.Invoke(Value);
        return new Receipt(this, Receipt.Type.Add, amount, callerFilePath, callerLineNumber);
    }

    /// <summary>
    /// Multiply this <c>BuffableStat</c>'s value by <paramref name="amount"/>.
    /// </summary>
    public Receipt MultiplierBuff(
            float amount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
    ) {
        if (amount != 0) {
            multiplier *= amount;
            ValueUpdated.Invoke(Value);
            return new Receipt(this, Receipt.Type.Multiply, amount, callerFilePath, callerLineNumber);
        } else {
            Debug.LogError("BuffableStat does not support 0x multipliers.");
            return new Receipt(this, Receipt.Type.Multiply, 1, callerFilePath, callerLineNumber);
        }
    }
}
