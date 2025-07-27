
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import CartTable from './CartTable';

const CartView = () => {
  const [cart, setCart] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  // פונקציה לטעינת עגלה מהשרת
  const fetchCart = async () => {
    try {
      const res = await api.get('/cart');
      setCart(res.data);
    } catch {
      setCart([]);
    }
  };

  useEffect(() => {
    fetchCart();
  }, []);

  // הסרה מהעגלה + רענון
  const removeFromCart = async (itemId) => {
    await api.delete(`/cart/${itemId}`);
    fetchCart();
  };

  // פונקציה לחשיפת רענון חיצוני (למשל, הוספה מדף אחר)
  window.refreshCartView = fetchCart;

  // סיום הזמנה
  const finishOrder = async () => {
    setLoading(true);
    setError("");
    try {
      await api.post('/cart/finish');
      setCart([]);
      window.refreshCartView && window.refreshCartView();
      navigate('/purchases');
    } catch (e) {
      setError("שגיאה בסיום ההזמנה");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <h2 style={{ textAlign: 'center', color: '#1976d2', fontWeight: 700, fontSize: 32, marginBottom: 32 }}>העגלה שלי</h2>
      {error && <div style={{ background: '#ffebee', color: '#c62828', borderRadius: 8, padding: 12, marginBottom: 16, textAlign: 'center', fontWeight: 600 }}>{error}</div>}
      <CartTable cart={cart} onRemove={removeFromCart} />
      {cart.length > 0 && (
        <div style={{ display: 'flex', justifyContent: 'center', marginTop: 32 }}>
          <button
            onClick={finishOrder}
            style={{ background: 'linear-gradient(90deg,#43e97b 0%,#38f9d7 100%)', color: '#fff', border: 'none', borderRadius: 30, padding: '16px 48px', fontSize: 22, fontWeight: 700, boxShadow: '0 2px 12px #38f9d766', transition: 'transform 0.1s', cursor: loading ? 'not-allowed' : 'pointer', outline: 'none', display: 'flex', alignItems: 'center', gap: 12 }}
            disabled={loading}
            onMouseDown={e => e.currentTarget.style.transform = 'scale(0.97)'}
            onMouseUp={e => e.currentTarget.style.transform = 'scale(1)'}
          >
            <span className="pi pi-credit-card" style={{ fontSize: 24, marginLeft: 8 }}></span>
            {loading ? 'שולח...' : 'סיום הזמנה'}
          </button>
        </div>
      )}
    </div>
  );
};

export default CartView;
