// Use this for stats that other code may change
// Basic usage:

using UnityEngine;
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

// should this be a struct? idk, ask the profiler
public class BuffableStat {
    public enum Order { 
        AddThenMultiply, // bigger numbers, default
        MultiplyThenAdd // smaller numbers
    }
    private Order order;
    private float multiplier = 1;
    private float adds = 0;
    private float initial;
    public float Value { get {
        if (order == Order.AddThenMultiply)
            return (initial + adds) * multiplier;
        else
            return (initial * multiplier) + adds;
    } }

    public class Receipt {
        public enum Type { Add, Multiply }
        private float value;
        private Type type;
        BuffableStat bs;

        private bool unapplied = false;

        public Receipt(BuffableStat bs, Type type, float value) {
            this.bs = bs;
            this.value = value;
            this.type = type;
        }

        public void Unbuff() {
            if (!unapplied) {
                if (type == Type.Add)
                    bs.adds -= value;
                else if (type == Type.Multiply)
                    bs.multiplier /= value;
            } unapplied = true;
        }

        ~Receipt() { // destructor :P
            if (!unapplied) {
                Debug.LogError("Someone forgot to unapply a buff!");
            }
        }
    }

    public BuffableStat(float initialValue, Order order = Order.AddThenMultiply) {
        this.initial = initialValue;
        this.order = order;
    }

    public Receipt AddBuff(float amount) {
        adds += amount;
        return new Receipt(this, Receipt.Type.Add, amount);
    }

    public Receipt MultiplierBuff(float amount) {
        if (amount != 0) {
            multiplier *= amount;
            return new Receipt(this, Receipt.Type.Multiply, amount);
        } else {
            Debug.LogError("BuffableStat does not support 0x multipliers.");
            return new Receipt(this, Receipt.Type.Multiply, 1);
        }
    }
}