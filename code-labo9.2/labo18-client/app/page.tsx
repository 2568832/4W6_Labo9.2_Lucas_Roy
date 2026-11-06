"use client";

import { useEffect, useState } from "react";
import { useStimeAPI } from "./_hooks/use-stime-api";
import { Review } from "./_types/review";

export default function Home() {

  useEffect(() => {

    getReviews();

  }, []);

  const {
    // États
    reviews,
    // Reviews
    getReviews, postReview, upvoteReview, deleteReview, editReview
  } = useStimeAPI();

  // Inputs pour ajouter critique
  const [gameName, setGameName] = useState("");
  const [reviewText, setReviewText] = useState("");

  // Inputs pour éditer critique
  const [toggleEdit, setToggleEdit] = useState<boolean>(false);
  const [editedText, setEditedText] = useState("");
  const [reviewId, setReviewId] = useState<number>(0);

  function startEdit(r: Review) {

    setToggleEdit(!toggleEdit);
    setEditedText(r.text);
    setReviewId(r.id);

  }

  function edit(r: Review) {

    editReview(r.id, editedText);
    setToggleEdit(!toggleEdit);

  }

  return (
    <div>
      <h3>Ajouter une critique</h3>
      <input type="text" placeholder="Nom du jeu" value={gameName} onChange={(e) => setGameName(e.target.value)} />
      <br />
      <textarea placeholder="Rédigez une critique du jeu ici ..." value={reviewText} onChange={(e) => setReviewText(e.target.value)}></textarea>
      <br />
      <button onClick={() => postReview(reviewText, gameName)}>Poster la critique</button>

      {reviews.length > 0 &&
        <div>
          <hr />
          <h3>Critiques</h3>
        </div>
      }

      {reviews.map((r, index) =>
        <div className="review" key={index}>
          <div className="author">{r.author} +{r.upvotes}</div>
          <div className="game">{r.game}</div>
          <div className="delete">
            <span onClick={() => upvoteReview(r.id)}>🔼</span>&nbsp;&nbsp;
            <span onClick={() => startEdit(r)}>📝</span>&nbsp;&nbsp;
            <span onClick={() => deleteReview(r.id)}>✖</span>
          </div>
          {(!toggleEdit || reviewId != r.id) &&
            <div>{r.text}</div>
          }
          {toggleEdit && reviewId == r.id &&
            <div>
              <textarea value={editedText} onChange={(e) => setEditedText(e.target.value)}>bla</textarea>
              <button onClick={() => edit(r)}>Modifier</button>
            </div>
          }

        </div>
      )}


    </div>
  );
}
