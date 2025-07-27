import React, { useState } from 'react';
import api from '../services/api';

const AddGiftToCart = () => {
  const [giftId, setGiftId] = useState('');
  const [quantity, setQuantity] = useState(1);

  const handleAdd = async () => {
    await api.post('/cart/add', { giftId, quantity });
    alert('התווסף לעגלה!');
  };

  return (
    <div style={{alignItems:'center', display: 'flex', flexDirection: 'column', gap: '10px'}}>
      <input value={giftId} onChange={e => setGiftId(e.target.value)} placeholder="מזהה מתנה" />
      <input type="number" value={quantity} onChange={e => setQuantity(Number(e.target.value))} min="1" />
      <button onClick={handleAdd}>הוסף לעגלה</button>
    </div>
  );
};

export default AddGiftToCart;
