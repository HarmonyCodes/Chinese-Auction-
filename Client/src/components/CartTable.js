import React from 'react';

const CartTable = ({ cart, onRemove }) => (
  <div style={{ overflowX: 'auto' }}>
    <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: 16, background: '#fff', borderRadius: 12, boxShadow: '0 2px 8px #e0e0e0' }}>
      <thead>
        <tr style={{ background: 'linear-gradient(90deg,#43e97b 0%,#38f9d7 100%)' }}>
          <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>שם מתנה</th>
          <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>כמות</th>
          <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>מחיר</th>
          <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>סטטוס</th>
          <th style={{ padding: 12, border: 'none', color: '#fff', fontWeight: 700 }}>הסר</th>
        </tr>
      </thead>
      <tbody>
        {cart.length === 0 ? (
          <tr>
            <td colSpan={5} style={{ textAlign: 'center', color: '#888', padding: 24, fontSize: 18 }}>העגלה ריקה</td>
          </tr>
        ) : (
          cart.map((item, idx) => (
            <tr key={item.id != null ? `${item.id}_${item.giftId ?? ''}` : Math.random()} style={{ background: idx % 2 === 0 ? '#f9f9f9' : '#fff' }}>
              <td style={{ padding: 12, border: 'none', fontWeight: 500 }}>{item.gift?.name || item.giftName || ''}</td>
              <td style={{ padding: 12, border: 'none' }}>{item.quantity}</td>
              <td style={{ padding: 12, border: 'none' }}>{item.gift?.price || item.price || ''}</td>
              <td style={{ padding: 12, border: 'none' }}>
                {item.isDraft === true || item.IsDraft === true ? (
                  <span style={{ color: '#1976d2', fontWeight: 700, letterSpacing: 1 }}>טיוטה</span>
                ) : (
                  <span style={{ color: '#388e3c', fontWeight: 700, letterSpacing: 1 }}>סופי</span>
                )}
              </td>
              <td style={{ padding: 12, border: 'none' }}>
                <button
                  style={{ background: 'linear-gradient(90deg,#d32f2f 0%,#ff6b6b 100%)', color: '#fff', border: 'none', borderRadius: '50%', width: 36, height: 36, display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer', boxShadow: '0 2px 6px #d32f2f33', fontSize: 18, transition: 'background 0.2s' }}
                  onClick={() => onRemove(item.id)}
                  title="הסר פריט"
                >
                  <span className="pi pi-trash"></span>
                </button>
              </td>
            </tr>
          ))
        )}
      </tbody>
    </table>
  </div>
);

export default CartTable;
