import React from "react";
import { Link } from "@reach/router";
import {
  Grid,
  Card,
  CardHeader,
  CardContent,
  Typography
} from "@material-ui/core";

type Props = {
  listing: {
    title: string,
    price: string,
    id: string,
    isbn10: String,
    isbn13: String,
    condition: string,
    authors: [string]
  }
};

const ListingCard = ({
  listing: { title, price, id, isbn10, isbn13, condition, authors }
}: Props) => (
  <Grid item>
    {/* //$FlowFixMe */}
    <Link to={`/listing/${id}`}>
      <Card raised>
        <CardHeader
          title={
            <>
              <Typography variant="h5">{title}</Typography>
              <Typography variant="subtitle1">
                By: {authors.join(",")}
              </Typography>
            </>
          }
        />
        <CardContent>
          <Typography variant="body1">{condition}</Typography>
          <Typography variant="body1">{isbn10}</Typography>
          <Typography variant="body1">{isbn13}</Typography>
          <Typography variant="body1">${price}</Typography>
        </CardContent>
      </Card>
    </Link>
  </Grid>
);

export default ListingCard;
