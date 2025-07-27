import React from 'react';
import GiftWinnerLabel from './GiftWinnerLabel';

const GiftCard = ({ gift, onAdd, winner }) => (
  <div style={{
    border: '1px solid #ccc',
    borderRadius: '8px',
    padding: '16px',
    margin: '8px',
    width: '220px',
    boxShadow: '0 2px 8px #eee',
    display: 'inline-block',
    verticalAlign: 'top',
    background: '#fff'
  }}>
    <h3 style={{ margin: '0 0 8px 0' }}>{gift.name}</h3>
    <div>קטגוריה: {gift.categoryName}</div>
    <div>מחיר: {gift.price} ₪</div>
    <div>תורם: {gift.donorName}</div>
    {gift.raffled || gift.Raffled ? (
      <>
        <div style={{ marginTop: '12px', color: '#d32f2f', fontWeight: 600 }}>המתנה הוגרלה</div>
        <GiftWinnerLabel winner={winner} />
      </>
    ) : (
      <button style={{ marginTop: '12px', background: '#1976d2', color: '#fff', border: 'none', borderRadius: '4px', padding: '8px 16px', cursor: 'pointer' }} onClick={() => onAdd(gift.id)}>
        הוסף לסל
      </button>
    )}
  </div>
);

export default GiftCard;
