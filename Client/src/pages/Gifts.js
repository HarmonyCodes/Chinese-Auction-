import React, { useEffect, useState } from 'react';
import api from '../services/api';
import GiftCard from '../components/GiftCard';
import CartTable from '../components/CartTable';

import GiftWinnerLabel from '../components/GiftWinnerLabel';

const Gifts = () => {
  const [gifts, setGifts] = useState([]);
  const [cart, setCart] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [winners, setWinners] = useState([]);

  useEffect(() => {
    fetchGifts();
    fetchCart();
    fetchWinners();
  }, []);

  const fetchWinners = async () => {
    try {
      const res = await api.get('/raffle');
      setWinners(res.data);
    } catch {}
  };

  const fetchGifts = async () => {
    setLoading(true);
    try {
      const res = await api.get('/gifts');
      setGifts(res.data);
    } catch (err) {
      if (err.response && err.response.status === 401) {
        setError('יש להתחבר כדי לצפות במתנות');
      } else if (err.response && err.response.status === 403) {
        setError('אין לך הרשאה לצפות במתנות');
      } else {
        setError('שגיאה בטעינת מתנות');
      }
    } finally {
      setLoading(false);
    }
  };

  const fetchCart = async () => {
    try {
      const res = await api.get('/cart');
      setCart(res.data);
    } catch (err) {
      if (err.response && err.response.status === 401) {
        setError('יש להתחבר כדי לצפות בעגלה');
      } else if (err.response && err.response.status === 403) {
        setError('אין לך הרשאה לעגלה');
      } else {
        setCart([]);
      }
    }
  };

  const addToCart = async (giftId) => {
    try {
      await api.post('/cart/add', { giftId, quantity: 1 });
      fetchCart();
      if (window.refreshCartView) window.refreshCartView();
      setSuccess('המתנה נוספה לעגלה!');
      setTimeout(() => setSuccess(''), 2000);
      setTimeout(() => {
        const cartHeader = document.getElementById('cart-header');
        if (cartHeader) cartHeader.scrollIntoView({ behavior: 'smooth' });
      }, 300);
    } catch (err) {
      if (err.response && err.response.status === 401) {
        setError('יש להתחבר כדי להוסיף לעגלה');
      } else if (err.response && err.response.status === 403) {
        setError('אין לך הרשאה להוסיף לעגלה');
      } else if (err.response && err.response.status === 409) {
        setError('לא ניתן להוסיף לעגלה מתנה שכבר הוגרלה');
      } else {
        setError('שגיאה בהוספה לסל');
      }
    }
  };

  const removeFromCart = async (itemId) => {
    try {
      await api.delete(`/cart/${itemId}`);
      fetchCart();
    } catch (err) {
      if (err.response && err.response.status === 401) {
        setError('יש להתחבר כדי להסיר מהעגלה');
      } else if (err.response && err.response.status === 403) {
        setError('אין לך הרשאה להסיר מהעגלה');
      } else if (err.response && err.response.status === 404) {
        setError('הפריט לא נמצא בעגלה');
      } else if (err.response && err.response.status === 409) {
        setError('ניתן למחוק רק פריט עגלה במצב טיוטה');
      } else {
        setError('שגיאה בהסרת פריט');
      }
    }
  };

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <h2>רשימת מתנות</h2>
      {loading && <div>טוען...</div>}
      {error && <div style={{ color: 'red' }}>{error}</div>}
      {success && <div style={{ color: 'green', fontWeight: 600 }}>{success}</div>}
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: 16 }}>
        {gifts.map(gift => {
          const winner = winners.find(w => w.giftId === gift.id || w.gift?.id === gift.id);
          return (
            <GiftCard key={gift.id} gift={gift} onAdd={addToCart} winner={winner} />
          );
        })}
      </div>
      <h2 id="cart-header" style={{ marginTop: 32 }}>העגלה שלי</h2>
      <CartTable cart={cart} onRemove={removeFromCart} />
    </div>
  );
};

export default Gifts;
