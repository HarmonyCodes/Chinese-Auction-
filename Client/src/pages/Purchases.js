import React, { useEffect, useState } from 'react';
import api from '../services/api';

const Purchases = () => {
  const [purchases, setPurchases] = useState([]);

  useEffect(() => {
    api.get('/purchases')
      .then(res => setPurchases(res.data))
      .catch(() => setPurchases([]));
  }, []);

  return (
    <div style={{ maxWidth: 900, margin: '0 auto', padding: 24 }}>
      <h2 style={{ textAlign: 'center', color: '#388e3c', fontWeight: 800, fontSize: 36, marginBottom: 40 }}>היסטוריית הזמנות</h2>
      {purchases.length === 0 ? (
        <div style={{ color: '#888', textAlign: 'center', marginTop: 32, fontSize: 20 }}>לא נמצאו הזמנות</div>
      ) : (
        purchases.map(purchase => (
          <div key={purchase.id} style={{ boxShadow: '0 2px 16px #b2dfdb55', borderRadius: 16, marginBottom: 32, padding: 24, background: '#fff' }}>
            <h3 style={{ color: '#1976d2', fontWeight: 700, marginBottom: 12 }}>הזמנה #{purchase.id} <span style={{ color: '#888', fontWeight: 400, fontSize: 18 }}>({new Date(purchase.date).toLocaleString('he-IL')})</span></h3>
            <table style={{ width: '100%', borderCollapse: 'collapse', background: '#fafafa', borderRadius: 8, overflow: 'hidden' }}>
              <thead>
                <tr style={{ background: 'linear-gradient(90deg,#43e97b 0%,#38f9d7 100%)' }}>
                  <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>שם מתנה</th>
                  <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>כמות</th>
                  <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>מחיר</th>
                </tr>
              </thead>
              <tbody>
                {purchase.purchaseGifts.map((item, idx) => (
                  <tr key={item.id} style={{ background: idx % 2 === 0 ? '#f9f9f9' : '#fff' }}>
                    <td style={{ padding: 12, border: 'none', fontWeight: 500 }}>{item.gift?.name || ''}</td>
                    <td style={{ padding: 12, border: 'none' }}>{item.quantity}</td>
                    <td style={{ padding: 12, border: 'none' }}>{item.gift?.price || ''}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ))
      )}
    </div>
  );
};

export default Purchases;
